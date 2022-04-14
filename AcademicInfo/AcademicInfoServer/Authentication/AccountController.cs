using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AcademicInfoServer.Authentication
{
    [Route("api/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        public AccountController(IJwtAuthenticationManager jwtAuthenticationManager)
        {
            this._jwtAuthenticationManager = jwtAuthenticationManager;
        }

        // GET: api/<NameController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/testget5
        [Authorize]
        //currently not getting the authorize pass even with bearer token
        [HttpGet("testget{value}")]
        public string Get(int value)
        {
            return "Received value: " + value.ToString();
        }


        //POST /api/authenticate

        [AllowAnonymous]
        [HttpPost("authenticate") ]
        public IActionResult Authenticate([FromBody] AccountCredentials userCred)
        {
            SecurityToken token = _jwtAuthenticationManager.Authenticate(userCred.UserID, userCred.Password);
            if (token == null)
            {
                return Unauthorized();
            }
            else
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var response = new
                {
                    Message = "User Authenticated Successfully!",
                    AccountID = userCred.UserID,
                    Token = tokenHandler.WriteToken(token),
                    Issued = token.ValidFrom,
                    Expires = token.ValidTo
                };
                string jsonResponse = JsonConvert.SerializeObject(response);
                return Ok(jsonResponse);
            }    
        }

    }
}
