using AutoMapper;
using YZPortal.Core.Domain.Database.Users;

namespace YZPortal.API.Controllers.Users
{
    public class MappingProfile : Profile
	{
		public MappingProfile()
		{
            // Create
            CreateMap<User, Create.Model>();

            // Index
            CreateMap<User, Index.Model>();

			// Details
			CreateMap<User, Details.Model>();

			// Delete
			CreateMap<User, Delete.Model>();

			// Update
			CreateMap<Update.Request, User>(MemberList.Source);
			CreateMap<User, Update.Model>();
		}
	}
}
