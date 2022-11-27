using System.Net;
using DUTPS.API.Dtos.Profile;
using DUTPS.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUTPS.API.Controllers
{
  public class UserController : BaseController
  {
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
      _userService = userService;
    }
    /// <summary>
    /// Get list falculties
    /// <para>Created at: 2022-11-28</para>
    /// <para>Created by: CoNT</para>
    /// </summary>
    /// <returns>List of Users</returns>
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
    [HttpGet()]
    [Authorize]
    [ProducesResponseType(typeof(ProfileDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetFaculties()
    {
      return Ok(await _userService.GetUsers());
    }
  }
}
