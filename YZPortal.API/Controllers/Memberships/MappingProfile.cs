using AutoMapper;
using YZPortal.API.Controllers.Memberships;
using YZPortal.API.Controllers.Memberships.ContentAccessLevels;
using YZPortal.API.Controllers.Memberships.DealerRoles;
using YZPortal.Core.Domain.Database.Memberships;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.FullStackCore.Enums.Memberships;

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
            CreateMap<Create.Request, UserInvite>();
            CreateMap<UserInvite, Create.Model>();

            // Update
            CreateMap<Membership, Update.Model>();

            // Etc
            CreateMap<MembershipDealerRole, MembershipDealerRoleViewModel>();
            CreateMap<DealerRole, DealerRoleViewModel>()
                .ForMember(c => c.Name, opt => opt.MapFrom(src => ((DealerRoleNames)src.Name).ToString()))
                .ForMember(c => c.Code, opt => opt.MapFrom(src => src.Name));
            CreateMap<MembershipContentAccessLevel, MembershipContentAccessLevelViewModel>();
            CreateMap<ContentAccessLevel, ContentAccessLevelViewModel>()
                .ForMember(c => c.Name, opt => opt.MapFrom(src => ((ContentAccessLevelNames)src.Name).ToString()))
                .ForMember(c => c.Code, opt => opt.MapFrom(src => src.Name));
        }
    }
}
