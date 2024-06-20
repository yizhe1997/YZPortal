using Application.Interfaces.Repositories;
using Application.Models;
using Application.Requests.Indexes;
using AutoMapper;
using Domain.Entities.Products;
using MediatR;

namespace Application.Features.Products.Queries.GetProductCategoriesExport
{
    public class GetProductCategoriesExportQuery : SearchRequest, IRequest<SearchResult<ProductCategoryExportDto>>
    {
    }

    public class GetProductCategoriesExportQueryHandler(
            IUnitOfWork<Guid> unitOfWork,
            IMapper mapper
        ) : IRequestHandler<GetProductCategoriesExportQuery, SearchResult<ProductCategoryExportDto>>
    {
        public async Task<SearchResult<ProductCategoryExportDto>> Handle(GetProductCategoriesExportQuery query, CancellationToken cancellationToken)
        {
            var result = await SearchResult<ProductCategory>.SuccessAsync<ProductCategoryExportDto>(query,
                unitOfWork.Repository<ProductCategory>().Entities.Select(x => new ProductCategory
                {
                    Name = x.Name,
                    IsPublished = x.IsPublished,
                    DisplayOrder = x.DisplayOrder
                }),
                mapper,
                x => x.Name.Contains(query.SearchString),
                cancellationToken: cancellationToken);

            return result;
        }
    }
}
