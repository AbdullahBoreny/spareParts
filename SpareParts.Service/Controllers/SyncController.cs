using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
            if (model == null)
            {
                return BadRequest("Model is null — JSON not bound");
            }
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

                                string password = reader.GetString(passwordID);
                                string username = reader.GetString(userNameID);

                                bool isValid = BCrypt.Net.BCrypt.Verify(UserPassword, password);

                                if (!isValid) return Unauthorized(new { Success = false, Message = "Invalid Password"});

                                return Ok(new { Success = true, Username = username});
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
            string LastNmae = "";

            firstName = splitname[0];
            if (splitname.Length > 1)
            {
                LastNmae = splitname[1];
            }

            DateTime UserCreationDate = new DateTime();
            string shortName = firstName.Substring(0,2) + LastNmae.Substring(0,2);
            bool isAllowed = true;


            try
            {
                using (SqlConnection connection = new SqlConnection("SpareParts"))
                {
                    connection.Open();
                    using (SqlCommand command1 = new SqlCommand())
                    {
                        command1.Connection = connection;
                        command1.CommandType = System.Data.CommandType.Text;
                        command1.CommandText = "SELECT 1 FROM Security.Users" +
                            "WHERE UserEmail= @userEmail";
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
                            command.CommandText = "INSERT INTO Security.Users" +
                                "(" +
                                    "UserID" +
                                    ", UserName" +
                                    ", UserNameShort" +
                                    ", UserEmail" +
                                    ", UserPassword" +
                                    ", UserMobileNumber" +
                                    ", UserGender" +
                                    ", UserCreationDate" +
                                    ", UserIsAuthenticated" +
                                ")" +
                                "Values" +
                                "(" +
                                    "@UserID" +
                                    ", @fullName" +
                                    ", @shortName" +
                                    ", @userEmail" +
                                    ", @userPassword" +
                                    ", @UserMobileNumber" +
                                    ", @UserGender" +
                                    ", @UserCreationDate" +
                                    ", @UserIsAuthenticated" +
                                ")";
                            command.Parameters.AddWithValue("@UserID", UserGuid);
                            command.Parameters.AddWithValue("@fullName", fullName);
                            command.Parameters.AddWithValue("@shortName", shortName);
                            command.Parameters.AddWithValue("@UserEmail", userEmail);
                            command.Parameters.AddWithValue("@userPassword", BCrypt.Net.BCrypt.HashPassword(userPassword));
                            command.Parameters.AddWithValue("@UserCreationDate", UserCreationDate);
                            command.Parameters.AddWithValue("@UserIsAuthenticated", true);
                            command.Parameters.AddWithValue("@UserRoleID", isShopOwner ? 3 : 2);

                            command.ExecuteNonQuery();
                        }
                        return Ok(new {Success = true});
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

}
