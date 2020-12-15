using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Services
{
    public interface IResourceService
    {
        public Task Update(int resourceType, int amount);
        public Task UpdateNetGeneration();
        public Task UpdateMaxAmount(int thLevel);
        public bool IsValid(int resourceType);
        public List<ResourceDTO> GetAllResources();
        public ResourceDTO GetResource(int resourceType);
        public int GetResourceAmount(int resourceType);
        public int GetResourceNetGeneration(int resourceType);
    }
}
