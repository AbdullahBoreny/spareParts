using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace SpareParts.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            string Username = model.Email;
            string usedPassword = model.Password;


            using (SqlConnection connection = new SqlConnection("SpareParts"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT * FROM Security.Users WHERE UserEmail = @Username";
                    command.Parameters.AddWithValue("@Username", Username);

                    using(SqlDataReader reader = command.ExecuteReader())
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

                            return Ok(new { Success = true, Username = username, Email = userEmail});
                        }
                        else
                        {
                            return Unauthorized(new { Success = false, Message = "User Not Found"});
                        }
                    }
                }
            }
            return Unauthorized(new { Success = false, Message = "Database not found"});
        }
    }
}
