using AutoMapper;
using YZPortal.Core.Domain.Database.Dealers;

namespace YZPortal.Api.Controllers.Dealers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Create
            CreateMap<Create.Request, Dealer>();
            CreateMap<Dealer, Create.Model>();

            // Index
            CreateMap<Dealer, Index.Model>();
        }
    }
}
