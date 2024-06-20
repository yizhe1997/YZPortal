using Application.Interfaces.Repositories;
using Application.Models;
using Application.Requests.Indexes;
using AutoMapper;
using Domain.Entities.Products;
using MediatR;

namespace Application.Features.Products.Queries.GetProductsExport
{
    public class GetProductsExportQuery : SearchRequest, IRequest<SearchResult<ProductExportDto>>
    {
    }

    public class GetProductsExportQueryHandler(
            IUnitOfWork<Guid> unitOfWork,
            IMapper mapper
        ) : IRequestHandler<GetProductsExportQuery, SearchResult<ProductExportDto>>
    {
        public async Task<SearchResult<ProductExportDto>> Handle(GetProductsExportQuery query, CancellationToken cancellationToken)
        {
            var result = await SearchResult<Product>.SuccessAsync<ProductExportDto>(query,
                unitOfWork.Repository<Product>().Entities.Select(x => new Product
                {
                    Name = x.Name,
                    Sku = x.Sku,
                    IsPublished = x.IsPublished,
                    StockQuantity = x.StockQuantity,
                    Price = x.Price
                }),
                mapper,
                x => x.Name.Contains(query.SearchString) ||
                x.Sku.Contains(query.SearchString),
                cancellationToken: cancellationToken);

            return result;
        }
    }
}
