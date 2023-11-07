using Application.Interfaces.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly IGenericRepository<Domain.Entities.Misc.File, Guid> _repository;

        public FileRepository(IGenericRepository<Domain.Entities.Misc.File, Guid> repository)
        {
            _repository = repository;
        }
    }
}