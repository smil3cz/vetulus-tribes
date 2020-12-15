using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Helpers
{
    public class LocalAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public static readonly string username = "FirstTestUser";
        public const string AuthScheme = "LocalAuth";
        private readonly Claim DefaultUserClaim = new Claim("Username", username);

        public LocalAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authenticationTicket = new AuthenticationTicket(
                new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { DefaultUserClaim }, AuthScheme)),
                new AuthenticationProperties(),
                AuthScheme);
            return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
        }
    }
}
