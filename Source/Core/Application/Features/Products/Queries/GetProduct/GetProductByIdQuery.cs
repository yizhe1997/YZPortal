using Application.Interfaces.Repositories;
using Application.Models;
using AutoMapper;
using Domain.Entities.Products;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Products.Queries.GetProduct
{
    public class GetProductByIdQuery : IRequest<Result<GetProductByIdDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetProductQueryHandler : IRequestHandler<GetProductByIdQuery, Result<GetProductByIdDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Guid> _unitOfWork;

        public GetProductQueryHandler(
            IMapper mapper,
            IUnitOfWork<Guid> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper; ;
        }

        public async Task<Result<GetProductByIdDto>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<Product>().GetByIdAsync(query.Id, cancellationToken);

            var product = _mapper.Map<GetProductByIdDto>(entity);

            return await Result<GetProductByIdDto>.SuccessAsync(product);
        }
    }
}
