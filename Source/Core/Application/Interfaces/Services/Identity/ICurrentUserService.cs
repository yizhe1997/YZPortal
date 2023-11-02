namespace Application.Interfaces.Services.Identity
{
    public interface ICurrentUserService
    {
        public string? NameIdentifier { get; }
        public string? DisplayName { get; }
        public string? FirstName { get; }
        public string? LastName { get; }
        public string? AuthClassRef { get; }
        public string? Email { get; }
        public DateTime AuthTime { get; }
        public DateTime AuthExpireTime { get; }
        public string? IdpAccessToken { get; }
        public string? IdentityProvider { get; }
        public string? IpAddress { get; }
        //public IEnumerable<Claim> Claims { get; }
        public List<Domain.Entities.Users.Identity> Identities { get; }
    }
}
