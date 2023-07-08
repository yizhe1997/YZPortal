using AutoMapper;
using Microsoft.Graph.Models;
using YZPortal.FullStackCore.Models.Graph.Groups;

namespace YZPortal.API.Controllers.Graph.GraphGroups
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			// Index
			CreateMap<Group, GraphGroupModel>();
        }
	}
}
