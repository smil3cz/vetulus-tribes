using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs.KingdomDtos;
using GreenFoxAcademy.SpaceSettlers.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class KingdomController : Controller
    {
        private readonly IKingdomService kingdomService;
        private readonly ITimeService timeService;

        public KingdomController(IKingdomService kingdomService, ITimeService timeService)
        {
            this.kingdomService = kingdomService;
            this.timeService = timeService;
        }

        /// <summary>
        /// Returns user kingdom.
        /// </summary>
        /// <returns>Returns user kingdom.</returns>
        /// <response code="200">Returns the user kingdom</response>
        /// <response code="404">UserId not found</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseKingdomDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 404)]
        public async Task<IActionResult> GetKingdom(long? userId)
        {
            await timeService.UpdateResourceAmounts();
            if (userId == null)
            {
                var kingdomDto = new ResponseKingdomDto(await kingdomService.GetKingdom());
                return Ok(kingdomDto);
            }
            var kingdom = await kingdomService.GetKingdomByUser((long)userId);
            if (kingdom != null)
            {
                return Ok(new ResponseKingdomDto(kingdom));
            }
            return NotFound(new ResponseDto { Status = "error", Message = "UserId not found" });
        }

        /// <summary>
        /// Returns kingdom by Id.
        /// </summary>
        /// <returns>Returns kingdom.</returns>
        /// <response code="200">Returns the kingdom</response>
        /// <response code="404">Kingdom Id not found</response>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseKingdomDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 404)]
        public async Task<IActionResult> GetKingdomId([FromRoute] long id)
        {
            var kingdom = await kingdomService.GetKingdom(id);
            if (kingdom != null)
            {
                var kingdomDto = new ResponseKingdomDto(kingdom);
                return Ok(kingdomDto);
            }
            return NotFound(new ResponseDto { Status = "error", Message = "Kingdom Id not found" });
        }

        /// <summary>
        /// Change name or location of kingdom.
        /// </summary>
        /// <returns>Returns user kingdom.</returns>
        /// <response code="200">Returns the user kingdom</response>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(ResponseKingdomDto), 200)]
        public async Task<IActionResult> ModifyKingdom([FromBody] RequestKingdomDto kingdomDto)
        {
            var kingdom = await kingdomService.ModifyKingdom(kingdomDto);
            return Ok(kingdom);
        }
    }
}
