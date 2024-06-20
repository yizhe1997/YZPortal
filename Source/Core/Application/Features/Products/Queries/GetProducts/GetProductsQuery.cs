using Application.Interfaces.Repositories;
using Application.Models;
using Application.Requests.Indexes;
using AutoMapper;
using Domain.Entities.Products;
using MediatR;

namespace Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQuery : SearchRequest, IRequest<SearchResult<ProductDto>>
    {
    }

    public class GetProductsQueryHandler(
            IUnitOfWork<Guid> unitOfWork,
            IMapper mapper
        ) : IRequestHandler<GetProductsQuery, SearchResult<ProductDto>>
    {
        public async Task<SearchResult<ProductDto>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var result = await SearchResult<Product>.SuccessAsync<ProductDto>(query,
                unitOfWork.Repository<Product>().Entities,
                mapper,
                x => x.Name.Contains(query.SearchString) ||
                x.Sku.Contains(query.SearchString),
                cancellationToken: cancellationToken);
            return result;
        }
    }
}
