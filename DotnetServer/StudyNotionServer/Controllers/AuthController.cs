using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using StudyNotionServer.Data;
using StudyNotionServer.ServiceLayer;
using StudyNotionServer.ServiceLayer.Models;
using Microsoft.AspNetCore.Authorization;

namespace StudyNotionServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IStudyNotionS _serviceLayer;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IStudyNotionS serviceLayer,ILogger<AuthController> logger) {

            _serviceLayer = serviceLayer;
            _logger = logger;
        }

        //POST: /api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            try
            {
                RegisterUserResponse response = await _serviceLayer.RegisterUser(request);

                if (response != null) 
                {
                    if (response.Success) 
                    {
                        return Ok(new{
                            success = response.Success,
                            message = "successfully registered the user",
                            data = response.RegisteredUser
                        });
                    }
                    else
                    {
                        if(! response.Success && string.Equals(response.Message.ToLower(), "invalid account type"))
                        {
                            return BadRequest(new
                            {
                                success = false,
                                message = "invalid account type"
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                success = response.Success,
                                message = response.Message
                            });
                        }
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,new
                    {
                        success = false,
                        message = "Internal server error"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured in Register method of AuthController. Exception - {ex.ToString()}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "exception occured"
                });
            }
        }


        //POST: /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            try
            {
                LoginUserResponse response = await _serviceLayer.LoginUser(request);

                if (response != null)
                {
                    if (response.Success && response.user != null)
                    {
                        // Build claims
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier,response.user.Id),
                            new Claim(ClaimTypes.Name, $"{response.user.FirstName} {response.user.LastName}"),
                            new Claim(ClaimTypes.Email, response.user.Email),
                            new Claim(ClaimTypes.Role, response.user.AccountType.ToString())
                        };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);

                        var props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(14)
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

                        return Ok(new
                        {
                            success = response.Success,
                            message = "login successful",
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            success = response.Success,
                            message = response.Message
                        });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        success = false,
                        message = "error occured"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured in Login method of AuthController. Exception - {ex.ToString()}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "exception occured"
                });
            }
        }


        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok(new {success = true, message = "logout successful" });
        }
    }
}
