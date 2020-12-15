using System.Threading.Tasks;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs.LeaderboardDtos;
using GreenFoxAcademy.SpaceSettlers.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenFoxAcademy.SpaceSettlers.Controllers
{
    [Route("leaderboard")]
    [ApiController]
    public class LeaderboardController : Controller
    {
        private readonly ILeaderboardService leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            this.leaderboardService = leaderboardService;
        }

        /// <summary>
        /// Returns leaderboard list of the best players by ships or buildings
        /// </summary>
        /// <returns>Returns leaderboard list of the best players by ships or buildings</returns>
        /// <response code="200">Returns leaderboard list of the best players by ships or buildings</response>
        [Authorize]
        [HttpGet("{leaderboardType}")] // is possible to use "ships" or "buildings"
        [ProducesResponseType(typeof(ResponseLeaderboardDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        public async Task<IActionResult> GetLeaderboardByShips(string leaderboardType)
        {
            if (leaderboardType.ToLower() == "ships" || leaderboardType.ToLower() == "buildings")
            {
                var leaderboardList = await leaderboardService.GetLeaderboard(leaderboardType);
                return Ok(new ResponseLeaderboardDto { Leaderboard = leaderboardList });
            }
            return BadRequest(new ResponseDto { Status = "Error", Message = "Only ships or buildings can be selected" });
        }
    }
}
