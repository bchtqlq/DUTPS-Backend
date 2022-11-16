using System.Net;
using System.Security.Claims;
using DUTPS.API.Dtos.Profile;
using DUTPS.API.Services;
using DUTPS.Commons.Enums;
using DUTPS.Commons.Schemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUTPS.API.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public ProfileController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        
        /// <summary>
        /// Get profile of user
        /// <para>Created at: 2022-11-15</para>
        /// <para>Created by: Quang TN</para>
        /// </summary>
        /// <returns>Data of user profile</returns>
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
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ProfileDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetInfo()
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                ProfileDto profile = await _authenticationService.GetProfile(username);
                if (profile == null)
                {
                    return NotFound();
                }
                return Ok(profile);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Error = e.Message });
            }
        }

        /// <summary>
        /// update profile of current user
        /// <para>Created at: 2022-11-15</para>
        /// <para>Created by: QuangTN</para>
        /// </summary>
        /// <param name="user">New data profile of user</param>
        /// <returns>Result after update</returns>
        /// <remarks>
        /// Mean of response.Code
        /// 
        ///     200 - Success
        ///     201 - Error validate input data
        ///     202 - Have error in code
        ///     403 - Not allow access this function
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
        ///         "Data": {}
        ///     }
        ///     
        /// Validate Error
        /// 
        ///     {
        ///         "Code": 201
        ///         "MsgNo": "",
        ///         "ListError": {
        ///             "Username": "E001",
        ///             "Email": "E005"
        ///         },
        ///         "Data": null
        ///     }
        ///     
        /// Have error in code
        /// 
        ///     {
        ///         "Code": 202
        ///         "MsgNo": "E014",
        ///         "ListError": null,
        ///         "Data": null
        ///     }
        ///     
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
        /// <response code="200">Result after update</response>
        /// <response code="401">not login yet</response>
        /// <response code="404">Not found profile</response>
        /// <response code="500">Have exception</response>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] UpdateProfileDto profile)
        {
            try
            {
                ResponseInfo response = new ResponseInfo();
                if (ModelState.IsValid)
                {
                    var username = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    response = await _authenticationService.UpdateProfile(username, profile);
                }
                else
                {
                    response.Code = CodeResponse.NOT_VALIDATE;
                    response.Message = "Invalid Input";
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Error = e.Message });
            }
        }

        /// <summary>
        /// change password of current user
        /// <para>Created at: 2022-11-15</para>
        /// <para>Created by: QuangTN</para>
        /// </summary>
        /// <param name="changePassword">New password of user</param>
        /// <returns>Result after update</returns>
        /// <remarks>
        /// Mean of response.Code
        /// 
        ///     200 - Success
        ///     201 - Error validate input data
        ///     202 - Have error in code
        ///     403 - Not allow access this function
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
        ///         "Data": {}
        ///     }
        ///     
        /// Validate Error
        /// 
        ///     {
        ///         "Code": 201
        ///         "MsgNo": "",
        ///         "ListError": {
        ///             "Username": "E001",
        ///             "Email": "E005"
        ///         },
        ///         "Data": null
        ///     }
        ///     
        /// Have error in code
        /// 
        ///     {
        ///         "Code": 202
        ///         "MsgNo": "E014",
        ///         "ListError": null,
        ///         "Data": null
        ///     }
        ///     
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
        /// <response code="200">Result after update</response>
        /// <response code="401">not login yet</response>
        /// <response code="404">Not found profile</response>
        /// <response code="500">Have exception</response>
        [HttpPost("ChangePassword")]
        [ProducesResponseType(typeof(ResponseInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                ResponseInfo response = new ResponseInfo();
                if (ModelState.IsValid)
                {
                    var username = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    response = await _authenticationService.ChangePassword(username, changePasswordDto);
                }
                else
                {
                    response.Code = CodeResponse.NOT_VALIDATE;
                    response.Message = "Invalid Input";
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Error = e.Message });
            }
        }
    }
}