using System.Threading.Tasks;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using GreenFoxAcademy.SpaceSettlers.Models.Entities;

namespace GreenFoxAcademy.SpaceSettlers.Services.Interfaces
{
    public interface IPurchaseService
    {
        Task<BuildingDto> PurchaseBuilding(string typeBuilding);
        Task<Ship> PurchaseNewShip(string shipType);
        Task<int?> UpgradeBuilding(long buildingId);

        Task ShipUpdate(long shipId);
    }
}
