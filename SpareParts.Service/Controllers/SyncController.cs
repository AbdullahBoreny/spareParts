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

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            string Username = model.Email;
            string usedPassword = model.Password;

            try
            {
                using (SqlConnection connection = new SqlConnection("SpareParts"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandText = "SELECT * FROM Security.Users WHERE UserEmail = @Username";
                        command.Parameters.AddWithValue("@Username", Username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int passwordID = reader.GetOrdinal("UserPassword");
                                int userShortID = reader.GetOrdinal("UserShortName");
                                int userNameID = reader.GetOrdinal("UserName");
                                int userEmailID = reader.GetOrdinal("UserEmail");
                                int userGenderID = reader.GetOrdinal("UserGenderID");
                                int userAuthID = reader.GetOrdinal("UserIsAuthenticated");

                                string password = reader.GetString(passwordID);
                                string userNameShort = reader.GetString(userShortID);
                                string username = reader.GetString(userNameID);
                                string userEmail = reader.GetString(userEmailID);
                                string userGender = reader.GetString(userGenderID);
                                string isAuthenticated = reader.GetString(userAuthID);



                                bool isValid = BCrypt.Net.BCrypt.Verify(usedPassword, password);

                                if (!isValid) return Unauthorized("Invalid Password");

                                return Ok(new { Success = true, Username = username, Email = userEmail });
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
                            command.Parameters.AddWithValue("@userPassword", userPassword);
                            command.Parameters.AddWithValue("@UserMobileNumber", "00000000000");
                            command.Parameters.AddWithValue("@UserGender", 1);
                            command.Parameters.AddWithValue("@UserCreationDate", UserCreationDate);
                            command.Parameters.AddWithValue("@UserIsAuthenticated", true);

                            command.ExecuteNonQuery();
                        }
                        using(SqlCommand command2 = new SqlCommand())
                        {
                            command2.CommandType = System.Data.CommandType.Text;
                            command2.CommandText = "INSERT INTO Security.UserRole " +
                                "(" +
                                    "UserRoleID" +
                                    ", UserID" +
                                    ", RoleID" +
                                    ", ModifiedOn" +
                                ")" +
                                "VALUES " +
                                "(" +
                                    "@UserRoleGuid" +
                                    ", @UserGuid" +
                                    ", @RoleID" +
                                    ", @UserCreationDate" +
                                ")";
                            command2.Parameters.AddWithValue("@UserRoleGuid", UserRoleGuid);
                            command2.Parameters.AddWithValue("@UserGuid", UserGuid);
                            command2.Parameters.AddWithValue("@RoleID", isShopOwner ? 3 : 2);
                            command2.Parameters.AddWithValue("@UserCreationDate", UserCreationDate);

                            command2.ExecuteNonQuery();
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


        public class LoginRequest
        {
            public string Email { get; }
            public string Password { get; }
        }

        public class SignupRequest
        {
            public string Email { get; set;}
            public string Password { get; set;}

            public string FullName { get; set;}

            public bool IsShopOwner { get; set;}

        }
    }

}
