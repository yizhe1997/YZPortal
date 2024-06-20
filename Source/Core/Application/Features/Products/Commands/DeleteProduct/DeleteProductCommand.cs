using Application.Interfaces.Repositories;
using Application.Models;
using Domain.Entities.Products;
using MediatR;

namespace Application.Features.Products.Commands.AddProduct
{
    public class DeleteProductCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteProductCommandCommandHandler(
        IUnitOfWork<Guid> unitOfWork) : IRequestHandler<DeleteProductCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.Repository<Product>().GetByIdAsync(command.Id, cancellationToken);

            if (product == null)
            {
                return await Result<Guid>.FailAsync("Product not found.");
            }

            await unitOfWork.Repository<Product>().DeleteAsync(product, cancellationToken);

            await unitOfWork.Commit(cancellationToken);

            return await Result<Guid>.SuccessAsync(product.Id, "Product deleted successfully.");
        }
    }
}
