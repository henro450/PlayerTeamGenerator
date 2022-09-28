using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace PlayerTeamGenerator.Helpers
{
    public class CustomAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public CustomAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = "Bearer SkFabTZibXE1aE14ckpQUUxHc2dnQ2RzdlFRTTM2NFE2cGI4d3RQNjZmdEFITmdBQkE=";

            var headerCheck = Request.Headers.ContainsKey("Authorization");
            if (!headerCheck)
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            var header = Request.Headers[HeaderNames.Authorization].ToString();
            if (header != token)
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "Authorized") };

            // generate claimsIdentity on the name of the class
            var claimsIdentity = new ClaimsIdentity(claims, nameof(CustomAuthHandler));

            // generate AuthenticationTicket from the Identity
            // and current authentication scheme
            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(claimsIdentity), this.Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
