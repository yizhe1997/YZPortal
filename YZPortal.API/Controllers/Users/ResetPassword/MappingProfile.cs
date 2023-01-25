using AutoMapper;
using YZPortal.Core.Domain.Database.Users;

namespace YZPortal.API.Controllers.Users.ResetPassword
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Create
            CreateMap<UserPasswordReset, Create.Model>();
        }
    }
}
