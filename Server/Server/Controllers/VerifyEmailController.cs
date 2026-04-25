using Microsoft.AspNetCore.Mvc;
using Server.ApiResponseObjects;
using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using System.Text.RegularExpressions;
using static Server.Database.Database;

namespace Server.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class VerifyEmailController : ControllerBase
    {
        [HttpGet("/api/verifyEmail")]
        public ActionResult<VerifyEmail> GetVerifyEmail(string email)
        {
            email = email.ToLower().Trim();

            Response.Headers.Add("Access-Control-Allow-Origin", "*");

            if (email.Length < 5 || email.Length > 64 || new Regex(@"^[\w\.@!#$%&*+\-/=?^_`{|}~]*$").Matches(email).Count == 0)
            {
                return new VerifyEmail()
                {
                    IsEmailVerified = false
                };
            }

            bool isEmailVerified = EmailExists(email);

            return new VerifyEmail()
            {
                IsEmailVerified = isEmailVerified
            };
        }
    }
}
