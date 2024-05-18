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

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, SearchResult<ProductDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Guid> _unitOfWork;

        public GetProductsQueryHandler(
            IMapper mapper,
            IUnitOfWork<Guid> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper; ;
        }

        public async Task<SearchResult<ProductDto>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var result = await SearchResult<Product>.SuccessAsync<ProductDto>(query,
                _unitOfWork.Repository<Product>().Entities,
                _mapper,
                x => x.Name.Contains(query.SearchString) ||
                x.Sku.Contains(query.SearchString),
                cancellationToken: cancellationToken);
            return result;
        }
    }
}
