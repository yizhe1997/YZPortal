using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;

namespace YZPortal.API.Infrastructure.Security.Authentication.BasicAuthentication
{
    // REF: https://stackoverflow.com/questions/38977088/asp-net-core-web-api-authentication?rq=4
    // https://stackoverflow.com/questions/35296648/basic-authentication-in-asp-net-core
    // I dont think this is safe, look into ceritificates and bearer 
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration configuration) : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var basicAuthenticationOptions = _configuration.GetSection("BasicAuthentication").Get<BasicAuthenticationOptions>();
            var authHeader = (string)Request.Headers.Authorization;

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            {
                //Extract credentials
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                int seperatorIndex = usernamePassword.IndexOf(':', StringComparison.OrdinalIgnoreCase);

                var username = usernamePassword.Substring(0, seperatorIndex);
                var password = usernamePassword.Substring(seperatorIndex + 1);

                //you also can use this.Context.Authentication here
                if (username == basicAuthenticationOptions.UserName && password == basicAuthenticationOptions.Password)
                {
                    var user = new GenericPrincipal(new GenericIdentity("User"), null);
                    var ticket = new AuthenticationTicket(user, new AuthenticationProperties(), "Basic");
                    return await Task.FromResult(AuthenticateResult.Success(ticket));
                }
                else
                {
                    return await Task.FromResult(AuthenticateResult.Fail("No valid user."));
                }
            }

            Response.Headers["WWW-Authenticate"] = "Basic realm=\"yourawesomesite.net\"";
            return await Task.FromResult(AuthenticateResult.Fail("No credentials."));
        }
    }

}
