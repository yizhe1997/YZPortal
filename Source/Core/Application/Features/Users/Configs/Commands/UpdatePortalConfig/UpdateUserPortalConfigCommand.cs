using Application.Interfaces.Repositories;
using Application.Models;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Text.Json.Serialization;

namespace Application.Features.Users.Configs.Commands.UpdatePortalConfig
{
    public class UpdateUserPortalConfigCommand : IRequest<Result>
    {
        [JsonIgnore]
        public string? UserSubId { get; set; }
        public string? Theme { get; set; }
        public bool UseTabSet { get; set; } = true;
        public bool IsFixedHeader { get; set; } = true;
        public bool IsFixedFooter { get; set; } = true;
        public bool IsFullSide { get; set; } = true;
        public bool ShowFooter { get; set; } = true;
    }

    public class UpdatePortalConfigCommandValidator : AbstractValidator<UpdateUserPortalConfigCommand>
    {
        public UpdatePortalConfigCommandValidator()
        {
            RuleFor(p => p.Theme)
            .NotEmpty()
            .MaximumLength(75);

            //TODO: add rule that the theme must exist in the db
        }

        //public async Task<bool> BeUniqueTitle(UpdateTodoListCommand model, string title, CancellationToken cancellationToken)
        //{
        //    return await _context.TodoLists
        //        .Where(l => l.Id != model.Id)
        //        .AllAsync(l => l.Title != title, cancellationToken);
        //}
    }

    public class UpdatePortalConfigCommandHandler(
        IUnitOfWork<Guid> unitOfWork,
        IStringLocalizer<SharedResource> localizer,
        IMapper mapper,
        IPortalConfigRepository portalConfigRepository) : IRequestHandler<UpdateUserPortalConfigCommand, Result>
    {
        public async Task<Result> Handle(UpdateUserPortalConfigCommand command, CancellationToken cancellationToken)
        {
            var portalConfig = await portalConfigRepository.GetByUserSubIdFirstOrDefaultAsync(command.UserSubId, cancellationToken);

            if (portalConfig == null)
                return await Result.FailAsync(localizer["Not Found"]);

            mapper.Map(command, portalConfig);

            await unitOfWork.Commit(cancellationToken);

            return await Result.SuccessAsync("Portal configuration updated successfully.");
        }
    }
}
