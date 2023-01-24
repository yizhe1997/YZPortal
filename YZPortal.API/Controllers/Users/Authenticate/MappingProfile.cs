using AutoMapper;

namespace YZPortal.API.Controllers.Users.Authenticate
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Dealer, DealerViewModel>();
        }
    }
}
