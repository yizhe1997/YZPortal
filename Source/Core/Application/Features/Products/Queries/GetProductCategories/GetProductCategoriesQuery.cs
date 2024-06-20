using Application.Interfaces.Repositories;
using Application.Models;
using Application.Requests.Indexes;
using AutoMapper;
using Domain.Entities.Products;
using MediatR;

namespace Application.Features.Products.Queries.GetProductCategories
{
    public class GetProductCategoriesQuery : SearchRequest, IRequest<Result<ProductCategoryDto>>
    {
        public string? UserSubId { get; set; }
    }

    public class GetCategoriesQueryHandler(
            IUnitOfWork<Guid> unitOfWork,
            IMapper mapper
        ) : IRequestHandler<GetProductCategoriesQuery, Result<ProductCategoryDto>>
    {
        public async Task<Result<ProductCategoryDto>> Handle(GetProductCategoriesQuery query, CancellationToken cancellationToken)
        {
            return await SearchResult<ProductCategory>.SuccessAsync<ProductCategoryDto>(query,
                unitOfWork.Repository<ProductCategory>().Entities,
                mapper,
                x => x.Name.Contains(query.SearchString),
                cancellationToken: cancellationToken);
        }
    }
}
