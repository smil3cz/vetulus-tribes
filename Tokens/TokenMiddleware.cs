using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Tokens
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate next;

        public TokenMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext dbContext, ITokenManager tokenManager)
        {
            if (string.IsNullOrEmpty(dbContext.Request.Headers["authorization"]))
            {
                await next(dbContext);

                return;
            }
            if (await tokenManager.IsCurrentActiveToken())
            {
                await next(dbContext);

                return;
            }
            dbContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
