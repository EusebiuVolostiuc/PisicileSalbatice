using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("testget{value}")]
        public string Get(int value)
        {
            return "Received value: " + value.ToString();
        }


        //POST /api/authenticate
        [Authorize]
        [AllowAnonymous]
        [HttpPost("authenticate") ]
        public IActionResult Authenticate([FromBody] AccountCredentials userCred)
        {
            string? token = this._jwtAuthenticationManager.Authenticate(userCred.UserID, userCred.Password);
            if (token == null)
            {
                return Unauthorized();
            }
            else
                return Ok(token);
        }
    }
}
