using Application.Interfaces.Contexts;
using FluentValidation;

namespace Application.Features.Products.Commands.AddProduct
{
    public class DeleteProductCommandValidator(IApplicationDbContext context) : AbstractValidator<DeleteProductCommand>
    {
    }
}
