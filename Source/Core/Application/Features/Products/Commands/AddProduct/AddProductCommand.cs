using Application.Interfaces.Repositories;
using Application.Models;
using AutoMapper;
using Domain.Entities.Products;
using MediatR;

namespace Application.Features.Products.Commands.AddProduct
{
    public class AddProductCommand : IRequest<Result<Guid>>
    {
        public string? Name { get; set; }
        public string? Sku { get; set; }
    }

    public class AddProductCommandCommandHandler : IRequestHandler<AddProductCommand, Result<Guid>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Guid> _unitOfWork;

        public AddProductCommandCommandHandler(
            IMapper mapper,
            IUnitOfWork<Guid> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper; ;
        }

        public async Task<Result<Guid>> Handle(AddProductCommand command, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<AddProductCommand, Product>(command);

            await _unitOfWork.Repository<Product>().AddAsync(product);

            await _unitOfWork.Commit(cancellationToken);

            return await Result<Guid>.SuccessAsync(product.Id, "Product Created.");
        }
    }
}
