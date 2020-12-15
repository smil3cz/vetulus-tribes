using System.Collections.Generic;
using System.Threading.Tasks;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using GreenFoxAcademy.SpaceSettlers.Models.Entities;

namespace GreenFoxAcademy.SpaceSettlers.Services
{
    public interface IBuildingService
    {
        Task<BuildingDto> CreateBuilding(string type);
        Task<List<BuildingDto>> GetBuildings();
        Task<BuildingDto?> FindBuilding(long buildingId);
        Task<BuildingDto?> ChangeBuildingLevel(Building building);
    }
}
