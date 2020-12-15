using GreenFoxAcademy.SpaceSettlers.Database;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using GreenFoxAcademy.SpaceSettlers.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Services
{
    public class ResourceService : IResourceService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly Kingdom kingdom;

        public ResourceService(ApplicationDbContext dbContext, IHttpContextAccessor contextAccessor)
        {
            this.dbContext = dbContext;
            var username = contextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Username").Value;
            kingdom = dbContext.Kingdoms.Include(k => k.User).Include(k => k.Resources).FirstOrDefault(k => k.User.Username == username);
        }

        public async Task Update(int resourceType, int amount)
        {
            var resource = kingdom.Resources.FirstOrDefault(r => r.Type == (ResourceType)resourceType);
            if (resource != null)
            {
                if (resource.Amount + amount <= resource.MaxAmount && resource.Amount + amount >= 0)
                {
                    resource.Amount += amount;
                }
                else if(resource.Amount + amount > resource.MaxAmount)
                {
                    resource.Amount = resource.MaxAmount;
                }
                resource.LastUpdated = DateTime.UtcNow;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateNetGeneration()
        {
            var thLevel = kingdom.Buildings.FirstOrDefault(b => b.Type == (BuildingType)0).Level;
            await FoodNetGeneration(thLevel);
            await GoldNetGeneration(thLevel);
        }

        private async Task FoodNetGeneration(int thLevel)
        {
            var food = kingdom.Resources.FirstOrDefault(r => r.Type == (ResourceType)0);
            var farms = kingdom.Buildings.Where(b => b.Type == (BuildingType)1 && b.FinishedAt.CompareTo(DateTime.UtcNow) <= 0).Select(f => f.Level).Sum();
            var ships = kingdom.Ships.Where(s => s.FinishedAt.CompareTo(DateTime.UtcNow) <= 0).Count();
            food.NetGeneration = (10 * (farms + thLevel)) - ships;
            await dbContext.SaveChangesAsync();
        }

        private async Task GoldNetGeneration(int thLevel)
        {
            var gold = kingdom.Resources.FirstOrDefault(r => r.Type == (ResourceType)1);
            var mines = kingdom.Buildings.Where(b => b.Type == (BuildingType)2 && b.FinishedAt.CompareTo(DateTime.UtcNow) <= 0).Select(m => m.Level).Sum();
            gold.NetGeneration = 10 * (mines + thLevel);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateMaxAmount(int thLevel)
        {
            var resources = kingdom.Resources.ToList();
            foreach(var r in resources)
            {
                r.MaxAmount = thLevel * 1000;
            }
            await dbContext.SaveChangesAsync();
        }

        public bool IsValid(int resourceType)
        {
            return Enum.IsDefined(typeof(ResourceType), resourceType);
        }

        public List<ResourceDTO> GetAllResources()
        {
            return kingdom != null ? kingdom.Resources.Select(r => new ResourceDTO(r)).ToList() : new List<ResourceDTO>();
        }

        public ResourceDTO? GetResource(int resourceType)
        {
            return IsValid(resourceType) ? new ResourceDTO(kingdom.Resources.FirstOrDefault(r => r.Type == (ResourceType)resourceType)) : (ResourceDTO)null;
        }

        public int GetResourceAmount(int resourceType)
        {
            var resource = kingdom.Resources.FirstOrDefault(r => r.Type == (ResourceType)resourceType);
            return resource != null ? resource.Amount : -1;
        }

        public int GetResourceNetGeneration(int resourceType)
        {
            var resource = kingdom.Resources.FirstOrDefault(r => r.Type == (ResourceType)resourceType);
            return resource != null ? resource.NetGeneration : -1;
        }
    }
}
