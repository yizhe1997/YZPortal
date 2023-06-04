using AutoMapper;
using Microsoft.Graph.Models;

namespace YZPortal.API.Controllers.Graph.GraphGroups
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			// Index
			CreateMap<Group, Index.Model>();
            CreateMap<DirectoryObject, Index.Model>();
        }
	}
}
