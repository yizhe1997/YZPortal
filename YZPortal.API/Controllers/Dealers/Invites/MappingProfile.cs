using AutoMapper;
using YZPortal.Core.Domain.Database.Memberships;

namespace YZPortal.API.Controllers.Dealers.Invites
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			// Index
			CreateMap<DealerInvite, Index.Model>();

			// Create
			CreateMap<DealerInvite, Create.Model>();
			CreateMap<Create.Request, DealerInvite>(MemberList.Source);

			// Index
			CreateMap<DealerInvite, Delete.Model>();
		}
	}
}
