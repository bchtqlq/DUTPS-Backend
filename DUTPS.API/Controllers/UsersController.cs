using System.Net;
using DUTPS.API.Dtos.Profile;
using DUTPS.API.Dtos.Users;
using DUTPS.API.Filters;
using DUTPS.API.Services;
using DUTPS.Commons.Enums;
using DUTPS.Commons.Schemas;
using Microsoft.AspNetCore.Mvc;
using Sentry;

namespace DUTPS.API.Controllers
{
    public class UsersController : BaseController
  {
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
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
    [AuthGroup(10)]
    [ProducesResponseType(typeof(ProfileDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetUsers()
    {
      return Ok(await _userService.GetUsers());
    }

    /// <summary>
    /// Get profile of user
    /// <para>Created at: 2022-11-28</para>
    /// <para>Created by: CoNT</para>
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
    [HttpGet("{id}")]
    [AuthGroup(10)]
    [ProducesResponseType(typeof(ProfileDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
      try
      {
        ProfileDto profile = await _userService.GetUserById(id);
        if (profile == null)
        {
          SentrySdk.CaptureMessage("Get data for Profile is invalid");
          return NotFound();
        }
        return Ok(profile);
      }
      catch (Exception e)
      {
        SentrySdk.CaptureMessage("Server error: " + e.Message);
        return StatusCode(500, new { Error = e.Message });
      }
    }

    /// <summary>
    /// user create
    /// <para>Created at: 2022-11-28</para>
    /// <para>Created by: CoNT</para>
    /// </summary>
    /// <param name="user">Data of user from manage screen</param>
    /// <returns>create success</returns>
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
    /// <response code="200">Result after create user</response>
    /// <response code="500">Have exception</response>
    [HttpPost()]
    [AuthGroup(10)]
    [ProducesResponseType(typeof(ResponseInfo), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateUpdateDto data)
    {
      ResponseInfo response = new ResponseInfo();
      try
      {
        if (ModelState.IsValid)
        {
          response = await _userService.CreateUser(data);
        }
        else
        {
          response.Code = CodeResponse.NOT_VALIDATE;
          response.Message = "Invalid Input";
        }
        if (response.Code == CodeResponse.OK) return Ok(response);
        if (response.Code == CodeResponse.NOT_VALIDATE) 
        {
            SentrySdk.CaptureMessage("Create User is invalid");
            return BadRequest(response);
        }
        if (response.Code == CodeResponse.HAVE_ERROR) 
        {
            SentrySdk.CaptureMessage("Server error");
            return BadRequest(response);
        }
        return Ok(response);
      }
      catch (Exception e)
      {
        SentrySdk.CaptureMessage("Server error: " + e.Message);
        return StatusCode(500, new { Error = e.Message });
      }
    }

    /// <summary>
    /// update profile of user
    /// <para>Created at: 2022-11-28</para>
    /// <para>Created by: CoNt</para>
    /// </summary>
    /// <param name="user">new data of user</param>
    /// <returns>result after update</returns>
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
    /// <response code="200">result after update</response>
    /// <response code="401">not login yet</response>
    /// <response code="500">have exception</response>
    [HttpPut("{id}")]
    [AuthGroup(10)]
    [ProducesResponseType(typeof(ResponseInfo), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserCreateUpdateDto profile)
    {
      try
      {
        ResponseInfo response = new ResponseInfo();
        if (ModelState.IsValid)
        {
          response = await _userService.UpdateUser(id, profile);
        }
        else
        {
          response.Code = CodeResponse.NOT_VALIDATE;
          SentrySdk.CaptureMessage("Create User is invalid");
        }
        return Ok(response);
      }
      catch (Exception e)
      {
        SentrySdk.CaptureMessage("Server error: " + e.Message);
        return StatusCode(500, new { Error = e.Message });
      }
    }

    /// <summary>
    /// Delete a User
    /// <para>Created at: 2022-11-28</para>
    /// <para>Created by: CoNt</para>
    /// </summary>
    /// <param name="id">id of User need delete</param>
    /// <returns>data after delete User</returns>
    /// <remarks>
    /// Mean of response.Code
    /// 
    ///     200 - Success
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
    /// Not have permission
    /// 
    ///     {
    ///         "Code": 403
    ///         "MsgNo": "E403",
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
    /// <response code="401">Not login yet</response>
    /// <response code="403">Not have permission</response>
    /// <response code="500">Have exception</response>
    [HttpDelete("{id}")]
    [AuthGroup(10)]
    [ProducesResponseType(typeof(ResponseInfo), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
      try
      {
        return Ok(await _userService.DeleteUser(id));
      }
      catch (Exception e)
      {
        SentrySdk.CaptureMessage("Server error: " + e.Message);
        return StatusCode(500, new { Error = e.Message });
      }
    }
  }
}
