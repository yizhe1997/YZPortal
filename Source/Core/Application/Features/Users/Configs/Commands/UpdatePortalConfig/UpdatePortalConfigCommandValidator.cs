using Application.Interfaces.Contexts;
using FluentValidation;

namespace Application.Features.Users.Configs.Commands.UpdatePortalConfig
{
    public class UpdatePortalConfigCommandValidator : AbstractValidator<UpdateUserPortalConfigCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdatePortalConfigCommandValidator(IApplicationDbContext context)
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
