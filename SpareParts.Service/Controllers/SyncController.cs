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
        public bool Login()
        {
            string Username = HttpContext.Request.Headers["Username"];
            string Password = HttpContext.Request.Headers["Password"];

            using(SqlConnection connection = new SqlConnection())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "spLogin";
                    command.Parameters.AddWithValue("username", Username);
                    command.Parameters.AddWithValue("password", Password);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return true;
                        }
                    }

                }
            }


            return false;
        }
    }
}
