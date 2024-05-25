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

    public class DeleteProductCommandCommandHandler : IRequestHandler<DeleteProductCommand, Result<Guid>>
    {
        private readonly IUnitOfWork<Guid> _unitOfWork;

        public DeleteProductCommandCommandHandler(
            IUnitOfWork<Guid> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(command.Id, cancellationToken);

            if (product == null)
            {
                return await Result<Guid>.FailAsync("Product not found");
            }

            await _unitOfWork.Repository<Product>().DeleteAsync(product);

            await _unitOfWork.Commit(cancellationToken);

            return await Result<Guid>.SuccessAsync(product.Id, "Product Deleted.");
        }
    }
}
