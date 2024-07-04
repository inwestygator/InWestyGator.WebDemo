using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using System;
using InWestyGator.WebDemo.Core.Contracts;

namespace InWestyGator.WebDemo.Handlers
{
    public class BasicAuthenticationHandler<TOptions> : AuthenticationHandler<TOptions>
            where TOptions : AuthenticationSchemeOptions, new()
    {
        private readonly IUserRepository _userRepository;
        public BasicAuthenticationHandler(
            IOptionsMonitor<TOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IUserRepository userRepository) :
           base(options, logger, encoder)
        {
            _userRepository = userRepository;
        }

        // very basic example to have some authentication lifecycle
        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            if (authorizationHeader != null && authorizationHeader.StartsWith("Basic", StringComparison.OrdinalIgnoreCase))
            {
                var token = authorizationHeader.Substring("Basic ".Length).Trim();
                var credentialsAsEncodedString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var credentials = credentialsAsEncodedString.Split(':');

                var user = await _userRepository.AuthenticateAsync(credentials[0], credentials[1]);
                if (user != null)
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        // if there are roles, we can add here them as well
                        // new Claim(ClaimTypes.Role, user.Role.ToString()), 
                        // we add the User Id for ease of access, more performant queryable
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };
                    var identity = new ClaimsIdentity(claims, "Basic");
                    var claimsPrincipal = new ClaimsPrincipal(identity);
                    return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
                }
            }
            Response.StatusCode = 401;
            Response.Headers["WWW-Authenticate"] = "Basic realm=\"github.com/inwestygator\"";
            return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        }
    }
}
