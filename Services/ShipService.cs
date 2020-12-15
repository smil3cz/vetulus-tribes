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
    public class ShipService : MasterService<Ship>, IShipService
    {
        private readonly IResourceService resourceService;

        public ShipService(ApplicationDbContext dbContext, IHttpContextAccessor contextAccessor, IResourceService resourceService) : base(dbContext, contextAccessor)
        {
            this.resourceService = resourceService;
        }

        public async Task<Ship> CreateNewShip(string type)
        {
            var shipType = (ShipType)Enum.Parse(typeof(ShipType), type);
            await dbContext.Ships.AddAsync(factory.Build(shipType, kingdom));
            await dbContext.SaveChangesAsync();
            await resourceService.UpdateNetGeneration();
            return await dbContext.Ships.OrderByDescending(s => s.Id).FirstOrDefaultAsync();
        }

        public async Task<List<Ship>> GetAllShips()
        {
            return await dbContext.Ships.Where(s => s.KingdomId == kingdom.Id).Select(s => s).ToListAsync();
        }

        public async Task<Ship> GetShipDetail(long shipId)
        {
            return await dbContext.Ships.FirstOrDefaultAsync(s => s.KingdomId == kingdom.Id && s.Id == shipId);
        }

        public async Task UpdateShipLevel(long shipId)
        {
            var ship = await dbContext.Ships.FirstOrDefaultAsync(s => s.Id == shipId);
            if (ship.Type != ShipType.Cruiser)
            {
                ship.Level++;
                ship.Attack++;
                ship.Defence++;
                dbContext.Update(ship);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                ship.Level++;
                ship.Attack += 2;
                ship.Defence += 2;
                dbContext.Update(ship);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
