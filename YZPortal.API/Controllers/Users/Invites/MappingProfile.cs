using AutoMapper;
using YZPortal.Core.Domain.Database.Users;

namespace YZPortal.API.Controllers.Users.Invites
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Index
            CreateMap<UserInvite, Index.Model>();

            // Create
            CreateMap<UserInvite, Create.Model>();
            CreateMap<Create.Request, UserInvite>(MemberList.Source);

            // Index
            CreateMap<UserInvite, Delete.Model>();
        }
    }
}
