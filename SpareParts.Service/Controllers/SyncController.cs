using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace SpareParts.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string  connectionString;

        public SyncController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("SpareParts");

        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            string Username = model.Email;
            string UserPassword = model.Password;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandText = "SELECT * FROM Security.Users WHERE UserEmail = @Username";
                        command.Parameters.AddWithValue("@Username", Username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int passwordID = reader.GetOrdinal("UserPassword");
                                int userNameID = reader.GetOrdinal("UserName");
                                int userIDOrdinal = reader.GetOrdinal("UserID");

                                string password = reader.GetString(passwordID);
                                string username = reader.GetString(userNameID);
                                string userID = reader.GetString(userIDOrdinal);
                                bool isValid = BCrypt.Net.BCrypt.Verify(UserPassword, password);

                                if (!isValid) return Unauthorized(new { Success = false, Message = "Invalid Password"});

                                return Ok(new { Success = true, Username = username, UserID = userID});
                            }
                            else
                            {
                                return Unauthorized(new { Success = false, Message = "User Not Found" });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        [HttpPost("Signup")]
        public IActionResult Signup([FromBody] SignupRequest model)
        {
            string fullName = model.FullName;
            string userEmail = model.Email;
            string userPassword = model.Password;
            bool isShopOwner = model.IsShopOwner;

            string UserGuid = Guid.NewGuid().ToString();
            string UserRoleGuid = Guid.NewGuid().ToString();

            string[] splitname = fullName.Split(' ');
            string firstName = "";
            string lastName = "";

            firstName = splitname[0];
            if (splitname.Length > 1)
            {
                lastName = splitname[1];
            }

            DateTime UserCreationDate = DateTime.Now;

            string shortName = (firstName.Length >= 2 ? firstName.Substring(0,2) : firstName) + (lastName.Length >=2 ? lastName.Substring(0,2) : lastName);
            bool isAllowed = true;
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command1 = new SqlCommand())
                    {
                        command1.Connection = connection;
                        command1.CommandType = System.Data.CommandType.Text;
                        command1.CommandText = "SELECT 1 FROM Security.Users " +
                            "WHERE UserEmail = @userEmail";
                        command1.Parameters.AddWithValue("@userEmail", userEmail);

                        using (SqlDataReader reader = command1.ExecuteReader()) 
                        {

                            if (reader.Read()) isAllowed = false;
                        }

                    }

                    if (isAllowed)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = connection;
                            command.CommandType = System.Data.CommandType.Text;
                            command.CommandText = "INSERT INTO Security.Users " +
                                "( " +
                                    "UserID" +
                                    ", UserName" +
                                    ", UserNameShort" +
                                    ", UserEmail" +
                                    ", UserPassword" +
                                    ", UserCreationDate" +
                                    ", UserRoleID " +
                                ") " +
                                "Values " +
                                "( " +
                                    "@UserID" +
                                    ", @fullName" +
                                    ", @shortName" +
                                    ", @userEmail" +
                                    ", @userPassword" +
                                    ", @UserCreationDate" +
                                    ", @UserRoleID " +
                                ")";
                            command.Parameters.AddWithValue("@UserID", UserGuid);
                            command.Parameters.AddWithValue("@fullName", fullName);
                            command.Parameters.AddWithValue("@shortName", shortName);
                            command.Parameters.AddWithValue("@UserEmail", userEmail);
                            command.Parameters.AddWithValue("@userPassword", BCrypt.Net.BCrypt.HashPassword(userPassword));
                            command.Parameters.AddWithValue("@UserCreationDate", UserCreationDate.ToString());
                            command.Parameters.AddWithValue("@UserRoleID", isShopOwner ? 3 : 2);

                            command.ExecuteNonQuery();
                        }
                        return Ok(new {Success = true, UserID = UserGuid});
                    }
                    else
                    {
                        return Ok(new { Success = false, Message = "A user with that Email Already Exists" });
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        [HttpPost("GetChats")]
        public IActionResult GetChats([FromBody] ChatsRequest model)
        {
            String userEmail = model.Email;
            Guid userID = model.UserID;
            List<ChatRecord> chatRecords = new List<ChatRecord>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "spGetUserChats";
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@UserEmail", userEmail);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int contactIdOrdinal = reader.GetOrdinal("ContactID");
                                int contactNameOrdinal = reader.GetOrdinal("ContactName");
                                int LastMessageOrdinal = reader.GetOrdinal("LastMessage");
                                int LastMessageTimeOrdinal = reader.GetOrdinal("LastMessageTime");
                                int UnreadCountOrdinal = reader.GetOrdinal("UnreadCount");

                                Guid contactID = reader.GetGuid(contactIdOrdinal);
                                string ContactName = reader.GetString(contactNameOrdinal);
                                string LastMessage = reader.GetString(LastMessageOrdinal);
                                DateTime LastMessageTime = reader.GetDateTime(LastMessageTimeOrdinal);
                                int UnreadCount = reader.GetInt32(UnreadCountOrdinal);

                                chatRecords.Add(new ChatRecord()
                                {
                                    ContactID = contactID,
                                    ContactName = ContactName,
                                    LastMessage = LastMessage,
                                    LastMessageTime = LastMessageTime,
                                    UnreadCount = UnreadCount
                                });
                            }
                            return Ok(new {Success = true, result =  chatRecords.ToString()});
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
    }
    public class LoginRequest
    {
        public string Email { get; set;}
        public string Password { get; set;}
    }

    public class SignupRequest
    {
        public string Email { get; set;}
        public string Password { get; set;}

        public string FullName { get; set;}

        public bool IsShopOwner { get; set;}

    }

    public class ChatsRequest
    {
        public string Email {get; set;}
        public Guid UserID {get; set;}
    }

    public class ChatRecord
    {
        public Guid ContactID {get; set;}
        public string ContactName {get; set;}
        public string LastMessage {get; set;}
        public DateTime LastMessageTime {get; set;}
        public int UnreadCount {get;set;}
    }

}
