using System.Net;
using DUTPS.API.Dtos.Authentication;
using DUTPS.API.Services;
using DUTPS.Commons.Enums;
using DUTPS.Commons.Schemas;
using Microsoft.AspNetCore.Mvc;
using Sentry;

namespace DUTPS.API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Check information of user login
        /// <para>Created at: 2022-11-08</para>
        /// <para>Created by: Quang TN</para>
        /// </summary>
        /// <param name="user">Data of user from login screen</param>
        /// <returns>Data token after login success</returns>
        /// <remarks>
        /// Mean of response.Code
        /// 
        ///     200 - Success
        ///     201 - Error validate input data
        ///     500 - Server error
        ///
        /// </remarks>
        /// <response code="200">
        /// Success
        /// 
        ///     {
        ///         "Code": 200,
        ///         "MsgNo": "",
        ///         "ListError": null,
        ///         "Data": {
        ///             "Token": "Token",
        ///             "Username": "Username",
        ///         }
        ///     }
        ///     
        /// Validate Error
        /// 
        ///     {
        ///         "Code": 201
        ///         "MsgNo": "",
        ///         "ListError": {
        ///         },
        ///         "Data": null
        ///     }
        /// Exception
        /// 
        ///     {
        ///         "Code": 500,
        ///         "MsgNo": "E500",
        ///         "ListError": null,
        ///         "Data": {
        ///             "Error": "Message"
        ///         }
        ///     }
        ///     
        /// </response>
        /// <response code="200">Result after check data login</response>
        /// <response code="500">Have exception</response>
        [HttpPost("Login")]
        [ProducesResponseType(typeof(ResponseInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            ResponseInfo response = new ResponseInfo();
            try
            {
                if (ModelState.IsValid)
                {
                    response = await _authenticationService.Login(userLoginDto);
                }
                else
                {
                    response.Code = CodeResponse.NOT_VALIDATE;
                    response.Message = "Invalid Input";
                }
                if (response.Code == CodeResponse.OK) return Ok(response);
                if (response.Code == CodeResponse.NOT_FOUND)
                {
                    SentrySdk.CaptureMessage("Login fail !!! \nWith data is invalid - Username:" + userLoginDto.Username + "  Password: " + userLoginDto.Password);
                    return NotFound(response);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Error = e.Message });
            }
        }

        /// <summary>
        /// user register
        /// <para>Created at: 2022-11-10</para>
        /// <para>Created by: Quang TN</para>
        /// </summary>
        /// <param name="user">Data of user from register screen</param>
        /// <returns>Data token after register success</returns>
        /// <remarks>
        /// Mean of response.Code
        /// 
        ///     200 - Success
        ///     201 - Error validate input data
        ///     500 - Server error
        ///
        /// </remarks>
        /// <response code="200">
        /// Success
        /// 
        ///     {
        ///         "Code": 200,
        ///         "MsgNo": "",
        ///         "ListError": null,
        ///         "Data": {
        ///             "Token": "Token",
        ///             "Username": "Username",
        ///         }
        ///     }
        ///     
        /// Validate Error
        /// 
        ///     {
        ///         "Code": 201
        ///         "MsgNo": "",
        ///         "ListError": {
        ///         },
        ///         "Data": null
        ///     }
        /// Exception
        /// 
        ///     {
        ///         "Code": 500,
        ///         "MsgNo": "E500",
        ///         "ListError": null,
        ///         "Data": {
        ///             "Error": "Message"
        ///         }
        ///     }
        ///     
        /// </response>
        /// <response code="200">Result after check data register</response>
        /// <response code="500">Have exception</response>
        [HttpPost("Register")]
        [ProducesResponseType(typeof(ResponseInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            ResponseInfo response = new ResponseInfo();
            try
            {
                if (ModelState.IsValid)
                {
                    response = await _authenticationService.Register(userRegisterDto);
                }
                else
                {
                    response.Code = CodeResponse.NOT_VALIDATE;
                    response.Message = "Invalid Input";
                }
                if (response.Code == CodeResponse.OK) return Ok(response);
                if (response.Code == CodeResponse.NOT_VALIDATE) 
                {
                    SentrySdk.CaptureMessage("Register fail !!! \nWith error status: " + response.Code);
                    return BadRequest(response);
                }
                if (response.Code == CodeResponse.HAVE_ERROR)
                {
                    SentrySdk.CaptureMessage("Register fail !!! \nWith error status: " + response.Code);
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureMessage(e.Message);
                return StatusCode(500, new { Error = e.Message });
            }
        }
    }
}