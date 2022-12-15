using System.Net;
using System.Security.Claims;
using DUTPS.API.Dtos.Vehicals;
using DUTPS.API.Services;
using DUTPS.Commons.Enums;
using DUTPS.Commons.Schemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sentry;

namespace DUTPS.API.Controllers
{
    public class VehicalsController : BaseController
    {
        private readonly IVehicalService _vehicalService;

        public VehicalsController(IVehicalService vehicalService)
        {
            _vehicalService = vehicalService;
        }
        
        /// <summary>
        /// Get vehicals of user
        /// <para>Created at: 2022-11-15</para>
        /// <para>Created by: Quang TN</para>
        /// </summary>
        /// <returns>Data of user vehicals</returns>
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
        [ProducesResponseType(typeof(List<VehicalDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var username = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                var vehicals = await _vehicalService.GetVehicalsByUsername(username);
                if (vehicals == null)
                {
                    return NotFound();
                }
                return Ok(vehicals);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureMessage("Server error: " + e.Message);
                return StatusCode(500, new { Error = e.Message });
            }
        }

        /// <summary>
        /// Get vehical of user by id
        /// <para>Created at: 2022-11-15</para>
        /// <para>Created by: Quang TN</para>
        /// </summary>
        /// <returns>Data of user vehicals</returns>
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
        [Authorize]
        [ProducesResponseType(typeof(List<VehicalDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById([FromRoute] long id)
        {
            try
            {
                var vehical = await _vehicalService.GetVehicalByVehicalId(id);
                if (vehical == null)
                {
                    return NotFound();
                }
                return Ok(vehical);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureMessage("Server error: " + e.Message);
                return StatusCode(500, new { Error = e.Message });
            }
        }

        /// <summary>
        /// create a vehical
        /// <para>Created at: 2022-15-11</para>
        /// <para>Created by: QuangTN</para>
        /// </summary>
        /// <param name="vehical">data of vehical</param>
        /// <returns>result after create</returns>
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
        /// <response code="200">result after create</response>
        /// <response code="401">not login yet</response>
        /// <response code="500">have exception</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ResponseInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] VehicalCreateUpdateDto vehical)
        {
            try
            {
                ResponseInfo response = new ResponseInfo();
                if (ModelState.IsValid)
                {
                    var username = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    response = await _vehicalService.AddVehical(username, vehical);
                }
                else
                {
                    response.Code = CodeResponse.NOT_VALIDATE;
                    response.Message = "Invalid Input";
                    SentrySdk.CaptureMessage("Create data for Vehical is invalid");
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
        /// update detail of a vehical
        /// <para>Created at: 2022-15-11</para>
        /// <para>Created by: QuangTN</para>
        /// </summary>
        /// <param name="vehical">new data of vehical</param>
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
        [Authorize]
        [ProducesResponseType(typeof(ResponseInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody] VehicalCreateUpdateDto vehical)
        {
            try
            {
                ResponseInfo response = new ResponseInfo();
                if (ModelState.IsValid)
                {
                    response = await _vehicalService.UpdateVehical(id, vehical);
                }
                else
                {
                    SentrySdk.CaptureMessage("Update data for Vehical is invalid");
                    response.Code = CodeResponse.NOT_VALIDATE;
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
        /// Delete a vehical
        /// <para>Created at: 2022-11-15</para>
        /// <para>Created by: QuangTN</para>
        /// </summary>
        /// <param name="id">id of vehical need delete</param>
        /// <returns>data after delete vehical</returns>
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
        [Authorize]
        [ProducesResponseType(typeof(ResponseInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            try
            {
                return Ok(await _vehicalService.DeleteVehical(id));
            }
            catch (Exception e)
            {
                SentrySdk.CaptureMessage("Server error: " + e.Message);
                return StatusCode(500, new { Error = e.Message });
            }
        }
    }
}