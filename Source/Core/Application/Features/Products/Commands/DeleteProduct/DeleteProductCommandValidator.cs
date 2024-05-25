using Application.Interfaces.Contexts;
using FluentValidation;

namespace Application.Features.Products.Commands.AddProduct
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteProductCommandValidator(IApplicationDbContext context)
        {
            _context = context;
        }
    }
}
