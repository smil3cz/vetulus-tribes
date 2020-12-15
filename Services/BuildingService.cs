using System;
using GreenFoxAcademy.SpaceSettlers.Database;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using GreenFoxAcademy.SpaceSettlers.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Services
{
    public class BuildingService : MasterService<Building>, IBuildingService
    {
        private readonly IResourceService resourceService;
        private readonly ITimeService timeService;

        public BuildingService(ApplicationDbContext dbContext, IHttpContextAccessor contextAccessor, IResourceService resourceService, ITimeService timeService) : base(dbContext, contextAccessor)

        {
            this.resourceService = resourceService;
            this.timeService = timeService;
        }

        public async Task<BuildingDto> CreateBuilding(string type)
        {
            await dbContext.Buildings.AddAsync(factory.Build((BuildingType)Enum.Parse(typeof(BuildingType), type), kingdom));

            await dbContext.SaveChangesAsync();
            if (type != "barracks")
            {
                await resourceService.UpdateNetGeneration();
            }
            var resultData = await dbContext.Buildings.Where(b => b.KingdomId == kingdom.Id).OrderByDescending(b => b.Id).FirstOrDefaultAsync();
            return new BuildingDto(resultData);
        }

        public async Task<List<BuildingDto>> GetBuildings()
        {
            var buildings = await dbContext.Buildings.Where(b => b.KingdomId == kingdom.Id).ToListAsync();
            var buildingsDto = buildings.Select(b => new BuildingDto(b)).ToList();
            return buildingsDto;
        }

        public async Task<BuildingDto?> FindBuilding(long buildingId)
        {
            var building = await dbContext.Buildings.FirstOrDefaultAsync(b => b.Id == buildingId);
            if (building.Kingdom == kingdom)
            {
                return new BuildingDto(building);
            }
            return null;
        }

        public async Task<BuildingDto?> ChangeBuildingLevel(Building building)
        {
            building.Level++;
            await resourceService.UpdateNetGeneration();
            if (building.Type == (BuildingType)0)
            {
                await timeService.UpdateResourceAmounts();
                await resourceService.UpdateNetGeneration();
                await resourceService.UpdateMaxAmount(building.Level);
            }
            await dbContext.SaveChangesAsync();
            return new BuildingDto(building);
        }
    }
}
