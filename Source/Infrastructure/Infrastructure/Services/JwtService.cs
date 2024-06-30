using Application.Interfaces.Services;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class JwtService(IOptions<JwtIssuerConfig> jwtConfig) : IJwtService
    {
        public async Task<string> CreateToken(string subject, List<Claim> claims)
        {
            claims ??= [];
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, subject));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, await jwtConfig.Value.JtiGenerator()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(jwtConfig.Value.IssuedAt).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));

            var jwt = new JwtSecurityToken(
                jwtConfig.Value.Issuer,
                jwtConfig.Value.Audience,
                claims,
                jwtConfig.Value.NotBefore,
                jwtConfig.Value.Expires,
                new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Value.SecretKey ?? string.Empty)), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }
    }
}
