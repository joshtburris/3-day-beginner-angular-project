using Microsoft.AspNetCore.Mvc;
using Server.ApiResponseObjects;
using System.Diagnostics;
using System.Net.Mime;
using System.Text.RegularExpressions;
using static Server.Database.Database;

namespace Server.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class VerifyLoginController : ControllerBase
    {
        [HttpGet("/api/verifyLogin")]
        public ActionResult<VerifyLogin> GetVerifyLogin(string email, string password)
        {
            email = email.ToLower().Trim();

            Response.Headers.Add("Access-Control-Allow-Origin", "*");

            if (    email.Length < 5 || email.Length > 64
                ||  password.Length < 1 || password.Length > 64
                ||  new Regex(@"^[\w\.@!#$%&*+\-/=?^_`{|}~]*$").Matches(email).Count == 0
                ||  new Regex(@"^[\w!#$%&*+\-/=?^_`{|}~]*$").Matches(password).Count == 0)
            {
                return new VerifyLogin(){};
            }

            string accessToken = AttemptLogin(email, password);
            if (string.IsNullOrEmpty(accessToken))
            {
                return new VerifyLogin() { };
            }

            return new VerifyLogin()
            {
                AccessToken = accessToken
            };
        }
    }
}
