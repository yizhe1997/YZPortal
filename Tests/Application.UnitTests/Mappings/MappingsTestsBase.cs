using AutoMapper;

namespace Application.UnitTests.Mappings
{
    public abstract class MappingsTestsBase<T> where T : Profile, new()
    {
        protected readonly IMapper _mapper;

        public MappingsTestsBase()
        {
            _mapper = new MapperConfiguration(x => { x.AddProfile<T>(); }).CreateMapper();
        }
    }
}
