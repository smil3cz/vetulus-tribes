using System.Linq;
using System.Threading.Tasks;
using GreenFoxAcademy.SpaceSettlers.Database;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using GreenFoxAcademy.SpaceSettlers.Models.Entities;
using GreenFoxAcademy.SpaceSettlers.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GreenFoxAcademy.SpaceSettlers.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IRestrictionsService restrictionsService;
        private readonly Kingdom kingdom;
        private readonly IBuildingService buildingService;
        private readonly IShipService shipService;

        public PurchaseService(ApplicationDbContext applicationDbContext, IRestrictionsService restrictionsService, IHttpContextAccessor httpContextAccessor, IBuildingService buildingService, IShipService shipService)
        {
            this.restrictionsService = restrictionsService;
            this.buildingService = buildingService;
            this.shipService = shipService;
            var user = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Username").Value;
            kingdom = applicationDbContext.Kingdoms.Include(k => k.User).Include(k => k.Ships).Include(k => k.Buildings).FirstOrDefault(k => k.User.Username == user);
        }

        public async Task<BuildingDto> PurchaseBuilding(string typeBuilding)
        {
            if (await restrictionsService.IsGoldForPurchaseBuilding(kingdom.Id, typeBuilding) && !string.IsNullOrEmpty(typeBuilding))
            {
                return await buildingService.CreateBuilding(typeBuilding);
            }
            return null;
        }

        public async Task<Ship> PurchaseNewShip(string shipType)
        {

            if (kingdom.Id > 0 &&
                kingdom.Buildings.Any(b => b.Type == BuildingType.barracks) &&
                restrictionsService.FoodRateProduction(kingdom.Id) &&
                await restrictionsService.GoldAvailableForShips(kingdom.Id, shipType)
                )
            {
                return await shipService.CreateNewShip(shipType);
            }
            return null;
        }

        public async Task<int?> UpgradeBuilding(long buildingId)
        {
            // Building not found = -1
            // Building upgraded = 1
            // Building not upgraded null
            var building = kingdom.Buildings.FirstOrDefault(b => b.Id == buildingId);
            if (building == null)
            {
                return -1;
            }

            if (restrictionsService.CheckGoldForUpgradeBuilding(kingdom, building) && await restrictionsService.CheckLevelForUpgrade(kingdom, building))
            {
                await buildingService.ChangeBuildingLevel(building);
                return 1;
            }
            return null;
        }

        public async Task ShipUpdate(long shipId)
        {
            var ship = kingdom.Ships.FirstOrDefault(s => s.Id == shipId);
            if (ship != null)
            {
                if (await restrictionsService.GoldAvailableForShips(kingdom.Id, ship.Type.ToString()))
                {
                    await shipService.UpdateShipLevel(shipId);
                }
            }
        }
    }
}
