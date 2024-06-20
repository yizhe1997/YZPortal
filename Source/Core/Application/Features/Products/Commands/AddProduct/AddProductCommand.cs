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

    public class AddProductCommandCommandHandler(
            IUnitOfWork<Guid> unitOfWork,
            IMapper mapper
        ) : IRequestHandler<AddProductCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(AddProductCommand command, CancellationToken cancellationToken)
        {
            var product = mapper.Map<AddProductCommand, Product>(command);

            await unitOfWork.Repository<Product>().AddAsync(product, cancellationToken);

            await unitOfWork.Commit(cancellationToken);

            return await Result<Guid>.SuccessAsync(product.Id, "Product created successfully.");
        }
    }
}
