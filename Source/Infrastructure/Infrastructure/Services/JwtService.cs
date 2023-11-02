using Application.Interfaces.Services;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtIssuerConfig _jwtConfig;

        public JwtService(IOptions<JwtIssuerConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig.Value;
        }

        public async Task<string> CreateToken(string subject, List<Claim> claims)
        {
            claims ??= new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, subject));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, await _jwtConfig.JtiGenerator()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(_jwtConfig.IssuedAt).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));

            var jwt = new JwtSecurityToken(
                _jwtConfig.Issuer,
                _jwtConfig.Audience,
                claims,
                _jwtConfig.NotBefore,
                _jwtConfig.Expires,
                new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey ?? string.Empty)), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }
    }
}
