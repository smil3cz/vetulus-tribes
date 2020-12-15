using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using GreenFoxAcademy.SpaceSettlers.Services;
using GreenFoxAcademy.SpaceSettlers.Services.Interfaces;
using GreenFoxAcademy.SpaceSettlers.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GreenFoxAcademy.SpaceSettlers.Controllers
{
    [Route("")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ITokenManager tokenManager;
        private readonly ILogsService logsService;

        public UserController(IUserService userDBService, ITokenManager tokenManager, ILogsService logsService)
        {
            this.userService = userDBService;
            this.tokenManager = tokenManager;
            this.logsService = logsService;
        }

        /// <summary>
        /// Register new user.
        /// </summary>
        /// <returns>Returns user dto</returns>
        /// <response code="200">Returns user dto.</response>
        /// <response code="400">Missing parameter(s).</response>
        /// <response code="409">Username already taken, please choose an other one.</response>
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        [ProducesResponseType(typeof(ResponseDto), 409)]
        public async Task<IActionResult> RegUser(UserDto input)
        {
            var user = await userService.GetUser(input);
            if (string.IsNullOrEmpty(input.Username) || string.IsNullOrEmpty(input.Password))
            {
                return BadRequest(new ResponseDto { Status = "error", Message = "Missing parameter(s)" });
            }
            else if (user != null)
            {
                return Conflict(new ResponseDto { Status = "error", Message = "Username already taken, please choose an other one." });
            }
            if (string.IsNullOrEmpty(input.KingdomName))
            {
                input.KingdomName = $"{input.Username}'s kingdom";
            }
            await userService.RegUser(input);
            var currentUser = await userService.GetUser(input);
            if (currentUser.Email != null)
            {
                await userService.SendConfirmationEmail(currentUser);
            }
            return Ok(new UserDto() { Id = currentUser.Id, Username = currentUser.Username, KingdomId = currentUser.Kingdom.Id });
        }

        /// <summary>
        /// Login user and return token.
        /// </summary>
        /// <returns>Returns token.</returns>
        /// <response code="200">Returns the user JWT token</response>
        /// <response code="400">Missing username.</response>
        /// <response code="401">Wrong username or password.</response>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponseDto), 200)]
        [ProducesResponseType(typeof(ResponseDto), 400)]
        [ProducesResponseType(typeof(ResponseDto), 401)]
        public async Task<IActionResult> LogUser(UserDto input)
        {
            if (string.IsNullOrEmpty(input.Username))
            {
                return BadRequest(new ResponseDto { Status = "error", Message = "Missing username" });
            }
            var user = await userService.AuthenticateUser(input);
            if (user == null)
            {
                return Unauthorized(new ResponseDto { Status = "error", Message = "Wrong username or password, or account not verified" });
            }
            var tokenString = await userService.GenerateJSONWebToken(user);
            await userService.SaveToken(tokenString);
            return Ok(new TokenResponseDto { Status = "Ok", Token = tokenString });
        }

        /// <summary>
        /// Logout user and forgets the token.
        /// </summary>
        /// <returns>Ok, succesfully logout.</returns>
        /// <response code="205">Succesful logout</response>
        [Authorize]
        [HttpDelete("logout")]
        [ProducesResponseType(205)]
        public async Task<NoContentResult> Logout()
        {
            await tokenManager.DeactivateCurrentAsync();
            return NoContent();
        }

        /// <summary>
        /// Verify email address with token
        /// </summary>
        /// <returns>Ok, succesfully logout.</returns>
        /// <response code="200">Succesful verification</response>
        /// <response code="400">Unsuccessful verification</response>
        [AllowAnonymous]
        [HttpGet("verify")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            return await userService.VerifyEmail(token) ? Ok("Email successfully verified") : (IActionResult)BadRequest("Email not verified");
        }

        /// <summary>
        /// See all logs saved in database.
        /// </summary>
        /// <returns>Ok, list of logs.</returns>
        /// <response code="200">Successfully retrieved logs</response>
        /// <response code="404">No logs found</response>
        [Authorize]
        [HttpGet("logs")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Logs()
        {
            var logs = logsService.RetrieveLogs();
            return logs.Any() ? Ok(JsonConvert.SerializeObject(logs)) : (IActionResult)NotFound("No logs found");
        }
    }
}
