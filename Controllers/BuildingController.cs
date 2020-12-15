using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs.BuildingsDtos;
using GreenFoxAcademy.SpaceSettlers.Services;
using GreenFoxAcademy.SpaceSettlers.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Controllers
{
    [Route("kingdom")]
    [ApiController]
    public class BuildingController : Controller
    {
        private string? message;
        private readonly IBuildingService buildingService;
        private readonly IPurchaseService purchaseService;
        private readonly ITimeService timeService;

        public BuildingController(IBuildingService buildingService, IPurchaseService purchaseService, ITimeService timeService)
        {
            this.buildingService = buildingService;
            this.purchaseService = purchaseService;
            this.timeService = timeService;
        }

        /// <summary>
        /// Returns user's list of buildings
        /// </summary>
        /// <returns>Returns user's list of buildings</returns>
        /// <response code="200">Returns user's list of buildings</response>
        [Authorize]
        [HttpGet("buildings")]
        [ProducesResponseType(typeof(BuildingsListDto), 200)]
        public async Task<IActionResult> GetBuildings()
        {
            var buildingsList = await buildingService.GetBuildings();
            return Ok(new BuildingsListDto { Buildings = buildingsList });
        }

        /// <summary>
        /// Purchase a new building
        /// </summary>
        /// <returns>Purchase a new building</returns>
        /// <response code="200">Purchase a new building according to specified type</response>
        /// <response code="400">Missing or invalid building type</response>
        [Authorize]
        [HttpPost("buildings")]
        [ProducesResponseType(typeof(BuildingDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> PurchaseBuilding(PurchaseBuildingDto buildingDto)
        {
            await timeService.UpdateResourceAmount(1);
            var type = buildingDto.Type;
            if (type == "farm" || type == "mine" || type == "barracks")
            {
                var resultData = await purchaseService.PurchaseBuilding(buildingDto.Type);
                if (resultData == null)
                {
                    return BadRequest(new ResponseDto { Status = "Error", Message = "You don't have enough of gold!" });
                }

                return Ok(resultData);

            }

            return StatusCode(400, string.IsNullOrEmpty(type)
                ? new ResponseDto { Status = "error", Message = "Missing parameter(s): type!" }
                : new ResponseDto { Status = "error", Message = "Invalid building type!" });
        }

        /// <summary>
        /// Returns building by Id
        /// </summary>
        /// <returns>Returns building by Id</returns>
        /// <response code="200">Returns building by Id</response>
        /// <response code="400">BuildingId not found</response>
        [Authorize]
        [HttpGet("buildings/{buildingId}")]
        [ProducesResponseType(typeof(BuildingDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 404)]
        public async Task<IActionResult> GetBuilding(long buildingId)
        {
            var buildingDto = await buildingService.FindBuilding(buildingId);
            if (buildingDto != null)
            {
                return Ok(buildingDto);
            }
            return StatusCode(404, new ResponseDto { Status = "error", Message = $"ID {buildingId} not found" });
        }

        /// <summary>
        /// Change building level
        /// </summary>
        /// <returns>Change building level</returns>
        /// <response code="200">Change building level</response>
        /// <response code="400">Missing, invalid or not found building level</response>
        /// /// <response code="404">Building ID does not belong to the logged in user</response>
        [Authorize]
        [HttpPost("buildings/{buildingId}")]
        [ProducesResponseType(typeof(BuildingDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        [ProducesResponseType(typeof(ResponseDto), 404)]
        public async Task<IActionResult> ChangeLevel(long buildingId)
        {
            var decider = await purchaseService.UpgradeBuilding(buildingId);
            if (decider == -1)
            {
                return StatusCode(404,
                    new ResponseDto
                    {
                        Status = "error",
                        Message =
                            $"Building ID {buildingId} does not belong to the logged user or building ID {buildingId} not found"
                    });
            }

            if (decider == null)
            {
                return StatusCode(400,
                    new ResponseDto
                    {
                        Status = "error",
                        Message = "You don't have enough of gold or town hall has insufficient level"
                    });
            }
            return StatusCode(200, buildingService.FindBuilding(buildingId));
        }
    }
}
