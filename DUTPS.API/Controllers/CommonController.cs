using System.Net;
using DUTPS.API.Dtos.Commons;
using DUTPS.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUTPS.API.Controllers
{
    public class CommonController : BaseController
    {
        private readonly ICommonService _commonService;

        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }
        /// <summary>
        /// Get list falculties
        /// <para>Created at: 2022-11-10</para>
        /// <para>Created by: Quang TN</para>
        /// </summary>
        /// <returns>List of falculties</returns>
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
        [HttpGet("Faculties")]
        [Authorize]
        [ProducesResponseType(typeof(FacultyDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFaculties()
        {
            return Ok(await _commonService.GetFaculties());
        }
    }
}