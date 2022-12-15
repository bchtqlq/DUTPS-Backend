using System.Net;
using System.Security.Claims;
using DUTPS.API.Dtos.Vehicals;
using DUTPS.API.Filters;
using DUTPS.API.Services;
using DUTPS.Commons;
using DUTPS.Commons.Enums;
using DUTPS.Commons.Schemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sentry;

namespace DUTPS.API.Controllers
{
    public class CheckInsController : BaseController
    {
        private readonly ICheckInService _checkInService;

        public CheckInsController(ICheckInService checkInService)
        {
            _checkInService = checkInService;
        }
        
        [HttpGet("GetById/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(CheckInDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCheckInByCheckInId([FromRoute] long id)
        {
            try
            {
                var checkIn = await _checkInService.GetCheckInByCheckInId(id);
                if (checkIn == null)
                {
                    return NotFound();
                }
                return Ok(checkIn);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureMessage("Server error: " + e.Message);
                return StatusCode(500, new { Error = e.Message });
            }
        }

        [HttpGet("GetByUsername")]
        [Authorize]
        [ProducesResponseType(typeof(CheckInDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCheckInByUserName([FromQuery] string username)
        {
            try
            {
                var currentUsername = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var checkIn = await _checkInService.GetCurrentCheckInByUsername(!string.IsNullOrEmpty(username) ? username : currentUsername);
                if (checkIn == null)
                {
                    return NotFound();
                }
                return Ok(checkIn);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureMessage("Server error: " + e.Message);
                return StatusCode(500, new { Error = e.Message });
            }
        }

        [HttpPost("CreateCheckIn")]
        [AuthGroup(new int[] {10, 20})]
        [ProducesResponseType(typeof(ResponseInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateCheckIn([FromBody] CheckInCreateDto checkIn)
        {
            try
            {
                ResponseInfo response = new ResponseInfo();
                if (ModelState.IsValid)
                {
                    var username = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    response = await _checkInService.CreateCheckIn(username, checkIn);
                }
                else
                {
                    response.Code = CodeResponse.NOT_VALIDATE;
                    response.Message = "Invalid Input";
                    SentrySdk.CaptureMessage("Create data for checkin is invalid");
                }
                return Ok(response);
                
            }
            catch (Exception e)
            {
                SentrySdk.CaptureMessage("Server error: " + e.Message);
                return StatusCode(500, new { Error = e.Message });
            }
        }

        [HttpPost("CreateCheckOut")]
        [AuthGroup(new int[] {10, 20})]
        [ProducesResponseType(typeof(ResponseInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateCheckOut([FromBody] CheckOutCreateDto checkOut)
        {
            try
            {
                ResponseInfo response = new ResponseInfo();
                if (ModelState.IsValid)
                {
                    var username = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    response = await _checkInService.CheckOut(username, checkOut);
                }
                else
                {
                    response.Code = CodeResponse.NOT_VALIDATE;
                    response.Message = "Invalid Input";
                    SentrySdk.CaptureMessage("Create data for checkout is invalid");
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureMessage("Server error: " + e.Message);
                return StatusCode(500, new { Error = e.Message });
            }
        }

        [HttpGet("History")]
        [AuthGroup(10)]
        [ProducesResponseType(typeof(PaginatedList<CheckInHistoryDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery] CheckInHistorySearchCondition condition)
        {
            try
            {
                var checkIns = await _checkInService.GetCheckInsHistory(condition);
                if (checkIns == null)
                {
                    return NotFound();
                }
                return Ok(checkIns);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureMessage("Server error: " + e.Message);
                return StatusCode(500, new { Error = e.Message });
            }
        }

        [HttpGet("AvailableCheckIns")]
        [AuthGroup(new int[] { 10, 20 })]
        [ProducesResponseType(typeof(PaginatedList<CheckInDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAvailableCheckIn([FromQuery] AvailableCheckInSearchCondition condition)
        {
            try
            {
                var checkIns = await _checkInService.GetAvailableCheckIn(condition);
                if (checkIns == null)
                {
                    return NotFound();
                }
                return Ok(checkIns);
            }
            catch (Exception e)
            {
                SentrySdk.CaptureMessage("Server error: " + e.Message);
                return StatusCode(500, new { Error = e.Message });
            }
        }
    }
}