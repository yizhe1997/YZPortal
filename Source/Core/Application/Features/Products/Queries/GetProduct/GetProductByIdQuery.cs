using Application.Interfaces.Repositories;
using Application.Models;
using AutoMapper;
using Domain.Entities.Products;
using MediatR;

namespace Application.Features.Products.Queries.GetProduct
{
    public class GetProductByIdQuery : IRequest<Result<GetProductByIdDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetProductQueryHandler(
            IUnitOfWork<Guid> unitOfWork,
            IMapper mapper
        ) : IRequestHandler<GetProductByIdQuery, Result<GetProductByIdDto>>
    {
        public async Task<Result<GetProductByIdDto>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var entity = await unitOfWork.Repository<Product>().GetByIdAsync(query.Id, cancellationToken);

            var product = mapper.Map<GetProductByIdDto>(entity);

            return await Result<GetProductByIdDto>.SuccessAsync(product);
        }
    }
}
