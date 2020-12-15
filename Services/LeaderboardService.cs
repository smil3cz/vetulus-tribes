using GreenFoxAcademy.SpaceSettlers.Database;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using GreenFoxAcademy.SpaceSettlers.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly ApplicationDbContext dbContext;

        public LeaderboardService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<LeaderDto>> GetLeaderboard(string leaderboardType)
        {
            var leaderboardList = new List<LeaderDto>();
            if (leaderboardType == "ships")
            {
                var kingdoms = await dbContext.Kingdoms.Include(k => k.Ships).OrderByDescending(k => k.Ships.Count).Take(10).ToListAsync();
                foreach (var kingdom in kingdoms)
                {
                    var leaderDto = new LeaderDto { KingdomName = kingdom.Name, Ships = kingdom.Ships.Count };
                    leaderboardList.Add(leaderDto);
                }
            }
            if (leaderboardType == "buildings")
            {
                var kingdoms = await dbContext.Kingdoms.Include(k => k.Buildings).OrderByDescending(k => k.Ships.Count).Take(10).ToListAsync();
                foreach (var kingdom in kingdoms)
                {
                    var leaderDto = new LeaderDto { KingdomName = kingdom.Name, Buildings = kingdom.Buildings.Count };
                    leaderboardList.Add(leaderDto);
                }
            }
            return leaderboardList;
        }
    }
}
