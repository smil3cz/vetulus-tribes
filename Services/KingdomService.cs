using GreenFoxAcademy.SpaceSettlers.Database;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs.KingdomDtos;
using GreenFoxAcademy.SpaceSettlers.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Services
{
    public class KingdomService : IKingdomService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly Kingdom currentkingdom;

        public KingdomService(ApplicationDbContext dbContext, IHttpContextAccessor contextAccessor)
        {
            this.dbContext = dbContext;
            var currentUsername = contextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Username").Value;
            currentkingdom = dbContext.Kingdoms.Include(k=>k.Buildings).Include(k=>k.Resources).Include(k=>k.Ships).FirstOrDefault(k => k.User.Username == currentUsername);
        }

        public async Task<Kingdom> GetKingdom()
        {
            return currentkingdom;
        }

        public async Task<Kingdom> GetKingdom(long kingdomId)
        {
            return await dbContext.Kingdoms.Include(k => k.Buildings).Include(k => k.Resources).Include(k => k.Ships).FirstOrDefaultAsync(k => k.Id == kingdomId);
        }

        public async Task<Kingdom> GetKingdomByUser(long userId)
        {
            return await dbContext.Kingdoms.Include(k => k.Buildings).Include(k => k.Resources).Include(k => k.Ships).FirstOrDefaultAsync(k => k.User.Id == userId);
        }

        public async Task<ResponseKingdomDto> ModifyKingdom(RequestKingdomDto kingdomDto)
        {
            if (!string.IsNullOrEmpty(kingdomDto.Name))
            {
                currentkingdom.Name = kingdomDto.Name;
            }
            if (kingdomDto.Location != null)
            {
                currentkingdom.Location = kingdomDto.Location;
            }
            if (!string.IsNullOrEmpty(kingdomDto.Name) || kingdomDto.Location != null)
            {
                dbContext.Kingdoms.Update(currentkingdom);
                await dbContext.SaveChangesAsync();
            }
            return new ResponseKingdomDto(currentkingdom);
        }
    }
}
