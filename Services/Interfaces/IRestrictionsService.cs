using GreenFoxAcademy.SpaceSettlers.Models.Entities;
using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Services.Interfaces
{
    public interface IRestrictionsService
    {
        bool FoodRateProduction(long kingdomId);
        Task<bool> IsGoldForPurchaseBuilding(long kingdomId, string buildingType);
        Task<bool> GoldAvailableForShips(long kingdomId, string shipType);
        Task<bool> CheckLevelForUpgrade(Kingdom kingdom, Building building);
        bool CheckGoldForUpgradeBuilding(Kingdom kingdom, Building building);
    }
}
