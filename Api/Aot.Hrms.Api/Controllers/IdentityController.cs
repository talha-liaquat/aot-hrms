using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Aot.Hrms.Contracts.Services;
using Aot.Hrms.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace Aot.Hrms.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public IdentityController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("authenticate")]
        [ProducesResponseType(typeof(LoginRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public IActionResult AuthenticateAsync([FromBody]LoginRequest request)
        {
            var user = _userService.Authenticate(request);

            if (user == null)
                return Unauthorized();
            
            var authResponse = new AuthenticationResponse
            {
                Token = GenerateToken(new List<Claim> {
                    new Claim("user-id", user.Value.userId),
                    new Claim("employee-id", user.Value.employeeId)
                }),

                UserId = user.Value.userId,
                EmployeeId = user.Value.employeeId
            };

            return Ok(authResponse);
        }

        private byte[] SecurityKey { get { return Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]); } }

        private string GenerateToken(List<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(SecurityKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(RegisterUserRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUserRequestAsync([FromBody]RegisterUserRequest request)
        {
            var token = await _userService.RegisterUserAsync(request);

            return Ok(token);
        }

        [HttpGet("callback")]
        [ProducesResponseType(typeof(RegisterUserRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> CallbackAsync(string state, string code, string scope, string authuser, string prompt)
        {
            string responseString;

            var postData = new StringBuilder();
            postData.Append(string.Format("{0}={1}&", HttpUtility.HtmlEncode("client_secret"), HttpUtility.HtmlEncode("FT7ygBBGXWuatfZxwK1BMD3-")));
            postData.Append(string.Format("{0}={1}&", HttpUtility.HtmlEncode("grant_type"), HttpUtility.HtmlEncode("authorization_code")));
            postData.Append(string.Format("{0}={1}&", HttpUtility.HtmlEncode("redirect_uri"), HttpUtility.HtmlEncode("http://localhost:61653/api/v1/Identity/callback")));
            postData.Append(string.Format("{0}={1}&", HttpUtility.HtmlEncode("code"), HttpUtility.HtmlEncode(code)));
            postData.Append(string.Format("{0}={1}", HttpUtility.HtmlEncode("client_id"), HttpUtility.HtmlEncode("430881244427-pr788bvcmlk0v3jm6mdo4n65gkkmqph3.apps.googleusercontent.com")));

            var content = new StringContent(postData.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");

            using (var httpClient = new HttpClient())
            {
                using var hmrcResponse = httpClient.PostAsync($"https://oauth2.googleapis.com/token", content).GetAwaiter().GetResult();
                responseString = hmrcResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (hmrcResponse.StatusCode != HttpStatusCode.OK)
                {
                    return new ContentResult
                    {
                        ContentType = "application/json",
                        Content = responseString,
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                }
            }

            var responseJObject = JObject.Parse(responseString);
            var accessToken = (string)responseJObject["access_token"];

            if (string.IsNullOrEmpty(accessToken))
            {
                return new ContentResult
                {
                    ContentType = "application/json",
                    Content = "Access token not found!",
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
            return Ok(responseString);
        }
    }
}
