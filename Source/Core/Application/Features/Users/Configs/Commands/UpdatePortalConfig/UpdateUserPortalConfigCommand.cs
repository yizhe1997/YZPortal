using Application.Interfaces.Repositories;
using Application.Models;
using AutoMapper;
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

    public class UpdatePortalConfigCommandHandler(
        IUnitOfWork<Guid> unitOfWork,
        IStringLocalizer<SharedResource> localizer,
        IMapper mapper,
        IPortalConfigRepository portalConfigRepository) : IRequestHandler<UpdateUserPortalConfigCommand, Result>
    {
        public async Task<Result> Handle(UpdateUserPortalConfigCommand command, CancellationToken cancellationToken)
        {
            var portalConfig = await portalConfigRepository.GetByUserSubIdFirstOrDefaultAsync(command.UserSubId);
            if (portalConfig != null)
            {
                mapper.Map(command, portalConfig);

                await unitOfWork.Commit(cancellationToken);

                return await Result.SuccessAsync("Portal configuration updated successfully.");
            }
            else
            {
                return await Result.FailAsync(localizer["Not Found"]);
            }
        }
    }
}
