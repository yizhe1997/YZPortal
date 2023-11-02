using System.Security.Claims;

namespace Application.Interfaces.Services
{
    public interface IJwtService
    {
        Task<string> CreateToken(string subject, List<Claim> claims);
    }
}
