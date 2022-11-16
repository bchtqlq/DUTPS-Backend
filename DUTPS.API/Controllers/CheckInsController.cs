using System.Net;
using System.Security.Claims;
using DUTPS.API.Dtos.Vehicals;
using DUTPS.API.Services;
using DUTPS.Commons.Enums;
using DUTPS.Commons.Schemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                return StatusCode(500, new { Error = e.Message });
            }
        }

        [HttpGet("GetByUsername")]
        [Authorize]
        [ProducesResponseType(typeof(CheckInDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCheckInByUserName()
        {
            try
            {
                var username = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var checkIn = await _checkInService.GetCurrentCheckInByUsername(username);
                if (checkIn == null)
                {
                    return NotFound();
                }
                return Ok(checkIn);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Error = e.Message });
            }
        }

        [HttpPost("CreateCheckIn")]
        [Authorize]
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
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Error = e.Message });
            }
        }

        [HttpPost("CreateCheckOut")]
        [Authorize]
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