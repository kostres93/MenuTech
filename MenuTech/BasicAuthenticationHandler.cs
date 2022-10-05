using MenuTech.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MenuTech
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly MenuTechContext _menuTechContext;
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            MenuTechContext menuTechContext) :base (options,logger,encoder,clock)
        {
            _menuTechContext = menuTechContext;
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization Header Missing"));
            }

            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var authorizationRegex = new Regex("Basic (.*)");

            if (!authorizationRegex.IsMatch(authorizationHeader))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization cone not formated."));
            }

            var authBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(authorizationRegex.Replace(authorizationHeader, "$1")));

            var authSplit = authBase64.Split(Convert.ToChar(":"), 2);
            var authUsername = authSplit[0];
            var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get password");

            var claim = new[] { new Claim(ClaimTypes.Name, authUsername) };
            var identity = new ClaimsIdentity(claim, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            if (_menuTechContext.Customers.Any(c=>c.UserName==authUsername && c.Password==authPassword))
            {
                
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail("ne moze"));

            //var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
            //return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal,Scheme.Name)));


        }
    }
}
