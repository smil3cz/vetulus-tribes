using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using GreenFoxAcademy.SpaceSettlers.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Controllers
{
    [Route("kingdom/resources")]
    [ApiController]
    public class ResourceController : Controller
    {
        private readonly IResourceService resourceService;
        private readonly ITimeService timeService;

        public ResourceController(IResourceService resourceService, ITimeService timeService)
        {
            this.resourceService = resourceService;
            this.timeService = timeService;
        }

        /// <summary>
        /// Returns all resources from specific kingdom.
        /// </summary>
        /// <returns>Returns list of all of specified kingdom's resources</returns>
        /// <response code="200">Returns the list of resources</response>
        /// <response code="404">Returns "No resources found."</response>
        [Authorize]
        [HttpGet("")]
        [ProducesResponseType(typeof(List<ResourceDTO>), 200)]
        [ProducesResponseType(typeof(ResponseDto), 404)]
        public async Task<IActionResult> GetAllResources()
        {
            await timeService.UpdateResourceAmounts();
            var resourceDTOs = resourceService.GetAllResources();
            return resourceDTOs.Any()
                ? Ok(Json(resourceDTOs))
                : (IActionResult)NotFound(new ResponseDto() { Status = "Error", Message = "No resources found." });
        }

        /// <summary>
        /// Returns specified resource data.
        /// </summary>
        /// <returns>Returns resourceDTO</returns>
        /// <response code="200">Returns resourceDTO</response>
        /// <response code="404">Returns "Resource not found"</response>
        [Authorize]
        [HttpGet("{resourceType}")]
        [ProducesResponseType(typeof(ResourceDTO), 200)]
        [ProducesResponseType(typeof(ResponseDto), 404)]
        public async Task<IActionResult> ResourceInformation([FromRoute] int resourceType)
        {
            await timeService.UpdateResourceAmount(resourceType);
            var resourceDTO = resourceService.GetResource(resourceType);
            return resourceDTO != null ? Ok(resourceDTO) : (IActionResult)NotFound(new { Status = "error", Message = "Resource not found." });
        }
    }
}
