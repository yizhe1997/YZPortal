using AutoMapper;

namespace YZPortal.API.Controllers.Users.ResetPassword
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Create
            CreateMap<PasswordReset, Create.Model>();
        }
    }
}
