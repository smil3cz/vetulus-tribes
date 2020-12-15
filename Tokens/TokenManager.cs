using GreenFoxAcademy.SpaceSettlers.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Tokens
{
    public class TokenManager : ITokenManager
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenManager()
        {

        }

        public TokenManager(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        private string GetCurrentAsync()
        {
            var authorizationHeader = httpContextAccessor.HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty ? string.Empty : authorizationHeader.Single().Split(" ").Last();
        }

        public async Task<bool> IsCurrentActiveToken()
        {
            return await IsActiveAsync(GetCurrentAsync());
        }

        public async Task<bool> IsActiveAsync(string token)
        {
            return await dbContext.Whitelist.FirstOrDefaultAsync(t => t.Jwt == token) != null;
        }

        public async Task DeactivateCurrentAsync()
        {
            await DeactivateAsync(GetCurrentAsync());
        }

        public async Task DeactivateAsync(string token)
        {
            var jwt = dbContext.Whitelist.FirstOrDefault(t => t.Jwt == token);
            dbContext.Whitelist.Remove(jwt);
            await dbContext.SaveChangesAsync();
        }
    }
}
