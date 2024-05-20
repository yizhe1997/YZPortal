using Application.Interfaces.Contexts;
using FluentValidation;

namespace Application.Features.Products.Commands.AddProduct
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        private readonly IApplicationDbContext _context;

        public AddProductCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            //TODO: add rule that the theme must exist in the db
        }

        //public async Task<bool> BeUniqueTitle(UpdateTodoListCommand model, string title, CancellationToken cancellationToken)
        //{
        //    return await _context.TodoLists
        //        .Where(l => l.Id != model.Id)
        //        .AllAsync(l => l.Title != title, cancellationToken);
        //}
    }
}
