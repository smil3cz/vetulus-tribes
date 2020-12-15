using System.Collections.Generic;
using System.Threading.Tasks;
using GreenFoxAcademy.SpaceSettlers.Models.Entities;

namespace GreenFoxAcademy.SpaceSettlers.Services
{
    public interface IShipService
    {
        Task<Ship> CreateNewShip(string shipType);
        Task<List<Ship>> GetAllShips();
        Task<Ship> GetShipDetail(long shipId);

        Task UpdateShipLevel(long shipId);
    }
}
