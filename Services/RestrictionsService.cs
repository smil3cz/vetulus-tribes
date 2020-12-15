using System.Linq;
using System.Threading.Tasks;
using GreenFoxAcademy.SpaceSettlers.Helpers;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using GreenFoxAcademy.SpaceSettlers.Models.Entities;
using GreenFoxAcademy.SpaceSettlers.Services.Interfaces;

namespace GreenFoxAcademy.SpaceSettlers.Services
{
    public class RestrictionsService : IRestrictionsService
    {
        private readonly IResourceService resourceService;
        private readonly PriceCollection priceCollection;

        public RestrictionsService(IResourceService resourceService, PriceCollection priceCollection)
        {
            this.resourceService = resourceService;
            this.priceCollection = priceCollection;
        }

        public bool FoodRateProduction(long kingdomId)
        {
            return resourceService.GetResourceNetGeneration(0) > 0;
        }

        public async Task<bool> IsGoldForPurchaseBuilding(long kingdomId, string buildingType)
        {
            var buildingPrice = priceCollection.PriceList.FirstOrDefault(k => k.Key == buildingType.ToLower()).Value;

            if (resourceService.GetResourceAmount(1) < buildingPrice)
            {
                return false;
            }
            await resourceService.Update(1, -buildingPrice);
            return true;
        }

        public async Task<bool> GoldAvailableForShips(long kingdomId, string shipType)
        {
            var shipPrice = priceCollection.PriceList.FirstOrDefault(k => k.Key == shipType.ToLower()).Value;

            if (resourceService.GetResourceAmount(1) < shipPrice)
            {
                return false;
            }

            await resourceService.Update(1, -shipPrice);
            return true;
        }

        public async Task<bool> CheckLevelForUpgrade(Kingdom kingdom, Building building)
        {
            var townHall = kingdom.Buildings.FirstOrDefault(b => b.Type == BuildingType.townhall);
            if (townHall.Level <= building.Level &&
                (building.Type != BuildingType.townhall || townHall.Level >= 20))
            {
                return false;
            }
            await resourceService.Update(1, -100 * building.Level);
            return true;
        }

        public bool CheckGoldForUpgradeBuilding(Kingdom kingdom, Building building)
        {
            return kingdom.Resources.FirstOrDefault(r => r.Type == ResourceType.gold).Amount >= 100 * building.Level;
        }
    }
}
