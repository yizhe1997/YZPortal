using Application.Features.Products.Queries.GetProductsExport;
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

    public class GetProductCategoriesExportQueryHandler : IRequestHandler<GetProductCategoriesExportQuery, SearchResult<ProductCategoryExportDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Guid> _unitOfWork;

        public GetProductCategoriesExportQueryHandler(
            IMapper mapper,
            IUnitOfWork<Guid> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper; ;
        }

        public async Task<SearchResult<ProductCategoryExportDto>> Handle(GetProductCategoriesExportQuery query, CancellationToken cancellationToken)
        {
            var result = await SearchResult<ProductCategory>.SuccessAsync<ProductCategoryExportDto>(query,
                _unitOfWork.Repository<ProductCategory>().Entities.Select(x => new ProductCategory
                {
                    Name = x.Name,
                    IsPublished = x.IsPublished,
                    DisplayOrder = x.DisplayOrder
                }),
                _mapper,
                x => x.Name.Contains(query.SearchString),
                cancellationToken: cancellationToken);

            return result;
        }
    }
}
