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

    public class GetCategoriesQueryHandler : IRequestHandler<GetProductCategoriesQuery, Result<ProductCategoryDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Guid> _unitOfWork;

        public GetCategoriesQueryHandler(
            IMapper mapper,
            IUnitOfWork<Guid> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper; ;
        }

        public async Task<Result<ProductCategoryDto>> Handle(GetProductCategoriesQuery query, CancellationToken cancellationToken)
        {
            var result = await SearchResult<ProductCategory>.SuccessAsync<ProductCategoryDto>(query,
                _unitOfWork.Repository<ProductCategory>().Entities,
                _mapper,
                x => x.Name.Contains(query.SearchString),
                cancellationToken: cancellationToken);
            return result;
        }
    }
}
