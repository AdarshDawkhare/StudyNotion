using System.Net;
using Microsoft.AspNetCore.Mvc;
using StudyNotionServer.Data;
using StudyNotionServer.ServiceLayer;
using StudyNotionServer.ServiceLayer.Models;

namespace StudyNotionServer.Controllers
{
    [ApiController]
    [Route("api/[cotroller]")]
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
                    if (response.Success)
                    {
                        return Ok(new
                        {
                            success = response.Success,
                            message = "successfully registered the user",
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


    }
}
