using System;
using System.Threading.Tasks;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using GreenFoxAcademy.SpaceSettlers.Services;
using GreenFoxAcademy.SpaceSettlers.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenFoxAcademy.SpaceSettlers.Controllers
{
    [Route("kingdom")]
    [ApiController]
    public class ShipController : ControllerBase
    {
        private readonly IShipService shipService;
        private readonly IPurchaseService purchaseService;
        private readonly ITimeService timeService;

        public ShipController(IShipService shipService, IPurchaseService purchaseService, ITimeService timeService)
        {
            this.shipService = shipService;
            this.purchaseService = purchaseService;
            this.timeService = timeService;
        }

        /// <summary>
        /// Creates ship for current kingdom.
        /// </summary>
        /// <returns>Returns new ship of kingdom.</returns>
        /// <response code="200">Returns new ship</response>
        /// <response code="400">Returns status with message.</response>
        [Authorize]
        [HttpPost("ship")]
        [ProducesResponseType(typeof(ShipDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> CreateShip(ShipDto type)
        {
            if (!Enum.IsDefined(typeof(ShipType), type.Type))
            {
                return BadRequest(new ResponseDto { Status = "error", Message = "Wrong type of ship" });
            }

            await timeService.UpdateResourceAmount(1);
            var ship = await purchaseService.PurchaseNewShip(type.Type);

            if (ship == null)
            {
                return BadRequest(new ResponseDto
                    {Status = "error", Message = "You don't have enough of gold or food"});
            }
            return Ok(new ShipDto(ship));
        }

        /// <summary>
        /// Returns all ships of current kingdom.
        /// </summary>
        /// <returns>Returns all ships of current kingdom.</returns>
        /// <response code="200">Returns all ships of current kingdom.</response>
        [Authorize]
        [HttpGet("ship")]
        [ProducesResponseType(typeof(ShipDto), 200)]
        public async Task<IActionResult> GetAllShips()
        {
            var ships = await shipService.GetAllShips();
            var shipsDto = ShipDto.ToListShipDto(ships);
            return Ok(new ShipDto { Ships = shipsDto });
        }

        /// <summary>
        /// Get selected ship of current kingdom.
        /// </summary>
        /// <returns>Returns selected ship of current kingdom.</returns>
        /// <response code="200">Returns ship details.</response>
        /// <response code="404">Returns Status status with message.</response>
        [Authorize]
        [HttpGet("ship/{shipId}")]
        [ProducesResponseType(typeof(ShipDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 404)]
        public async Task<IActionResult> GetShipDetail([FromRoute] long shipId)
        {
            var ship = await shipService.GetShipDetail(shipId);
            if (ship == null)
            {
                return NotFound(new ResponseDto { Status = "error", Message = $"{shipId} not found" });
            }
            return Ok(new ShipDto(ship));
        }

        /// <summary>
        /// Update specified ship level
        /// </summary>
        /// <returns>Return updated ship</returns>
        /// <response code="200">Return ship object</response>
        /// <response code="400">Return status with message</response>
        [Authorize]
        [HttpPost("ship/{shipId}")]
        [ProducesResponseType(typeof(ShipDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> UpdateShip([FromRoute] long shipId)
        {
            if (shipId <= 0)
            {
                return BadRequest(new ResponseDto {Status = "error", Message = "Wrong id provided!"});
            }

            await purchaseService.ShipUpdate(shipId);
            var shipResult = await shipService.GetShipDetail(shipId);
            return Ok(new ShipDto(shipResult));
        }
    }
}
