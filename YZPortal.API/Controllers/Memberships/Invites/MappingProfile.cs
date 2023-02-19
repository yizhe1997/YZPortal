using AutoMapper;
using YZPortal.Core.Domain.Database.Memberships;

namespace YZPortal.API.Controllers.Memberships.Invites
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Index
            CreateMap<MembershipInvite, Index.Model>();

            // Create
            CreateMap<MembershipInvite, Create.Model>();
            CreateMap<Create.Request, MembershipInvite>(MemberList.Source);

            // Index
            CreateMap<MembershipInvite, Delete.Model>();
        }
    }
}
