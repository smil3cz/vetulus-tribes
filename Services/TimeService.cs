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
    public class TimeService : ITimeService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IResourceService resourceService;
        private readonly Kingdom kingdom;

        public TimeService(ApplicationDbContext dbContext, IResourceService resourceService, IHttpContextAccessor contextAccessor)
        {
            this.dbContext = dbContext;
            this.resourceService = resourceService;
            var username = contextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Username").Value;
            kingdom = dbContext.Kingdoms.Include(k => k.User).Include(k => k.Resources).Include(k => k.Buildings).Include(k => k.Ships).FirstOrDefault(k => k.User.Username == username);
        }

        public async Task UpdateResourceAmounts()
        {
            var resources = kingdom.Resources.ToList();
            foreach(var r in resources)
            {
                await UpdateResourceAmount((int)r.Type);
            }
        }

        public async Task UpdateResourceAmount(int resourceType)
        {
            switch (resourceType)
            {
                case 0:
                    await UpdateFoodAmount();
                    break;
                case 1:
                    await UpdateGoldAmount();
                    break;
                default:
                    break;
            }
        }

        private async Task UpdateFoodAmount()
        {
            var food = kingdom.Resources.FirstOrDefault(r => r.Type == 0);
            var now = DateTime.UtcNow;
            var totalElapsedTime = (int)(now.Subtract(food.LastUpdated)).TotalMinutes;
            if(totalElapsedTime == 0)
            {
                return;
            }
            var additions = new List<KeyValuePair<string, DateTime>>();
            var newFarms = kingdom.Buildings.Where(b => b.Type == (BuildingType)1 && b.FinishedAt.CompareTo(food.LastUpdated) >= 0);
            if (newFarms.Any())
            {
                var newFarmsKVP = newFarms.OrderBy(f => f.FinishedAt)
                    .Select(f => new KeyValuePair<string, DateTime>("farm", f.FinishedAt));
                additions.AddRange(newFarmsKVP);
            }
            var newShips = kingdom.Ships.Where(s => s.FinishedAt.CompareTo(food.LastUpdated) >= 0);
            if (newShips.Any())
            {
                var newShipsKVP = newShips.OrderBy(s => s.FinishedAt)
                    .Select(s => new KeyValuePair<string, DateTime>("ship", s.FinishedAt));
                additions.AddRange(newShipsKVP);
                additions = additions.OrderBy(i => i.Value).ToList();
            }
            if (additions.Any())
            {
                var weigthedNetGen = (double)food.NetGeneration;
                foreach(var a in additions)
                {
                    switch (a.Key)
                    {
                        case "farm":
                            weigthedNetGen += ((10 * (int)(now.Subtract(a.Value)).TotalMinutes) / totalElapsedTime);
                            break;
                        case "ship":
                            weigthedNetGen -= (((int)(now.Subtract(a.Value)).TotalMinutes) / totalElapsedTime);
                            break;
                        default:
                            break;
                    }
                }
                await resourceService.Update(0, (int)(weigthedNetGen * (int)now.Subtract(food.LastUpdated).TotalMinutes));
            }
            else
            {
                await resourceService.Update(0, food.NetGeneration * (int)now.Subtract(food.LastUpdated).TotalMinutes);
            }
        }

        private async Task UpdateGoldAmount()
        {
            var gold = kingdom.Resources.FirstOrDefault(r => r.Type == (ResourceType)1);
            var now = DateTime.UtcNow;
            var totalElapsedTime = (int)(now.Subtract(gold.LastUpdated)).TotalMinutes;
            if(totalElapsedTime == 0)
            {
                return;
            }
            var newMines = kingdom.Buildings.Where(b => b.Type == (BuildingType)2 && b.FinishedAt.CompareTo(gold.LastUpdated) >= 0);
            if (newMines.Any())
            {
                var newMinesFA = newMines.Select(m => m.FinishedAt)
                    .OrderBy(m => m);
                var weigthedNetGen = (double)gold.NetGeneration;
                foreach (var m in newMinesFA)
                {
                    weigthedNetGen = totalElapsedTime == 0 ? weigthedNetGen = 1 : weigthedNetGen += ((10 * (int)(now.Subtract(m)).TotalMinutes) / totalElapsedTime);
                }
                await resourceService.Update(1, (int)(weigthedNetGen * (int)now.Subtract(gold.LastUpdated).TotalMinutes));
            }
            else
            {
                await resourceService.Update(1, gold.NetGeneration * (int)now.Subtract(gold.LastUpdated).TotalMinutes);
            }
        }
    }
}
