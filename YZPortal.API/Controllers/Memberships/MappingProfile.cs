using AutoMapper;
using YZPortal.Api.Controllers.ContentAccessLevels;
using YZPortal.Api.Controllers.DealerRoles;
using YZPortal.API.Controllers.Memberships;
using YZPortal.Core.Domain.Database.Memberships;

namespace YZPortal.Api.Controllers.Memberships
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Index
            CreateMap<Membership, Index.Model>();

            // Detail
            CreateMap<Membership, Details.Model>();

            // Delete
            CreateMap<Membership, Delete.Model>();

            // Create Invite
            CreateMap<Create.Request, MembershipInvite>();
            CreateMap<MembershipInvite, Create.Model>();

            // Update
            CreateMap<Membership, Update.Model>();

            // Etc
            CreateMap<MembershipDealerRole, DealerRolesViewModel>();
            CreateMap<DealerRole, DealerRolesViewModel>();
            CreateMap<MembershipContentAccessLevel, ContentAccessLevelsViewModel>();
            CreateMap<ContentAccessLevel, ContentAccessLevelsViewModel>();
        }
    }
}
