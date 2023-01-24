using AutoMapper;
using YZPortal.Api.Controllers.ContentAccessLevels;
using YZPortal.Api.Controllers.DealerRoles;
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

            // Create Invite Bulk
            CreateMap<CreateBulk.Sheet, MembershipInvite>();
            CreateMap<MembershipInvite, MembershipsCreateViewModel>();

            // Update
            CreateMap<Membership, Update.Model>();

            // Etc
            CreateMap<MembershipDealerRole, MembershipDealerRoleViewModel>();
            CreateMap<DealerRole, DealerRolesViewModel>();
            CreateMap<MembershipContentAccessLevel, MembershipContentAccessLevelViewModel>();
            CreateMap<ContentAccessLevel, ContentAccessLevelsViewModel>();
        }
    }
}
