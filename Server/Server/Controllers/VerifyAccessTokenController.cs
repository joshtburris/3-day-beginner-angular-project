using Microsoft.AspNetCore.Mvc;
using Server.ApiResponseObjects;
using System.Net.Mime;
using System.Text.RegularExpressions;
using static Server.Database.Database;

namespace Server.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class VerifyAccessTokenController : ControllerBase
    {
        [HttpGet("/api/verifyAccessToken")]
        public ActionResult<VerifyAccessToken> GetVerifyAccessToken(string accessToken)
        {
            accessToken = accessToken.ToLower();

            Response.Headers.Add("Access-Control-Allow-Origin", "*");

            if (accessToken.Length != 36 || new Regex(@"^[a-f0-9\-]*$").Matches(accessToken).Count == 0)
            {
                return new VerifyAccessToken()
                {
                    IsAccessTokenVerified = false
                };
            }

            bool isAccessTokenVerified = AccessTokenExists(accessToken);

            return new VerifyAccessToken()
            {
                IsAccessTokenVerified = isAccessTokenVerified
            };
        }
    }
}
