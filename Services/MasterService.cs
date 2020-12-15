using GreenFoxAcademy.SpaceSettlers.Database;
using GreenFoxAcademy.SpaceSettlers.Factories;
using GreenFoxAcademy.SpaceSettlers.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GreenFoxAcademy.SpaceSettlers.Services
{
    public class MasterService<T> where T : class
    {
        protected readonly Kingdom kingdom;
        protected readonly ApplicationDbContext dbContext;
        protected readonly IHttpContextAccessor contextAccessor;
        protected readonly ObjectFactory<T> factory;

        public MasterService(ApplicationDbContext dbContext, IHttpContextAccessor contextAccessor)
        {
            this.dbContext = dbContext;
            this.contextAccessor = contextAccessor;
            factory = new ObjectFactory<T>();

            var username = contextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Username").Value;
            kingdom = dbContext.Kingdoms.Include(k => k.Buildings).Include(k => k.Resources).Include(k => k.Ships).FirstOrDefault(k => k.User.Username == username);
        }
    }
}
