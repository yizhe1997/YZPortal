using AutoMapper;
using static YZPortal.API.Controllers.Memberships.Invites.Import;

namespace YZPortal.API.Controllers.Memberships.Invites
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Index
            CreateMap<Invite, Index.Model>();

            // Create
            CreateMap<Invite, Create.Model>();
            CreateMap<Invite, InviteViewModel>();
            CreateMap<Sheet, Invite>();
            CreateMap<Create.Request, Invite>(MemberList.Source);
            CreateMap<Sheet, Sheet>()
                .ForMember(x => x.Dealership, opt => opt.Ignore());

            // Index
            CreateMap<Invite, Delete.Model>();
        }
    }
}
