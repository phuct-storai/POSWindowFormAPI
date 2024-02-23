using Microsoft.AspNetCore.Mvc;
using POSWindowFormAPI.Data.Constants;
using POSWindowFormAPI.Models;
using POSWindowFormAPI.Models.Request;
using POSWindowFormAPI.Services.Interfaces;

namespace POSWindowFormAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public readonly ILogger<AccountController> _logger;
        public readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger,
                        IAccountService accountService) 
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpGet("get-by-username")]
        public Account GetByUsername(string username)
        {
            return _accountService.GetByUsername(username);
        }
        [HttpPost("register")]
        public ActionResult CreateAccount([FromBody] AuthenticationRequest authenticationRequest)
        {
            if (_accountService.CreateAccount(authenticationRequest) == AuthenticaticalConstants.SUCCESS)
                return Ok("Register Success - " + authenticationRequest.Username);

            else if (_accountService.CreateAccount(authenticationRequest) == AuthenticaticalConstants.EXITS_USERNAME)
                return BadRequest("Register Failed! Already exits this username: " + authenticationRequest.Username);

            else
                return BadRequest();
        }

        [HttpPut("update-username")]
        public ActionResult UpdateAccount(string username, [FromBody] AuthenticationRequest authenticationRequest)
        {
            if (_accountService.UpdateAccount(username, authenticationRequest))
                return Ok("Update Success"); 
            return BadRequest("Failed! Check again...");
        }

        [HttpDelete("delete-username")]
        public ActionResult DeleteAccount(string username)
        {
            if (_accountService.DeleteAccount(username))
                return Ok("Account Deleted - " + username);
            return BadRequest("Failed! Unavailable account.");
        }

        [HttpPost("login")]
        public ActionResult LoginAuthenticate([FromBody] AuthenticationRequest authenticationRequest)
        {
            if (authenticationRequest.Username == null || authenticationRequest.Password == null)
                return BadRequest("Username and Password cannot be empty!");

            else
            {
                if (_accountService.LoginValidate(authenticationRequest.Username, authenticationRequest.Password))
                    return Ok("Login Success - " + authenticationRequest.Username);
                return BadRequest("Login Failed! Check again with this user: " + authenticationRequest.Username);
            }
        }

        [HttpPost("forgot-password")]
        public ActionResult ForgotPassword()
        {
            return Ok();
        }
    }
}
