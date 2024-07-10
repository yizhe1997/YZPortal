using Application.Interfaces.Contexts;
using FluentValidation;

namespace Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandValidator(IApplicationDbContext context) : AbstractValidator<DeleteProductCommand>
    {
    }
}
