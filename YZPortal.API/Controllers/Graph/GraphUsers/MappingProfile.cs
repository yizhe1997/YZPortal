using AutoMapper;
using Microsoft.Graph.Models;
using YZPortal.FullStackCore.Models.Graph.Users;

namespace YZPortal.API.Controllers.Graph.GraphUsers
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			// Index
			CreateMap<User, GraphUserModel>();
		}
	}
}
