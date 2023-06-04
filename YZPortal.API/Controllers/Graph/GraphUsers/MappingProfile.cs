using AutoMapper;
using Microsoft.Graph.Models;

namespace YZPortal.API.Controllers.Graph.GraphUsers
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			// Index
			CreateMap<User, Index.Model>();
		}
	}
}
