using Application.Interfaces.Repositories;
using Application.Models;
using AutoMapper;
using MediatR;

namespace Application.Features.Users.Configs.Queries.GetConfigs
{
    public class GetUserConfigsQuery : IRequest<Result<ConfigsDto>>
    {
        public string? UserSubId { get; set; }
    }

    public class GetConfigsQueryHandler(
        IMapper mapper,
        IPortalConfigRepository portalConfigRepository) : IRequestHandler<GetUserConfigsQuery, Result<ConfigsDto>>
    {
        public async Task<Result<ConfigsDto>> Handle(GetUserConfigsQuery request, CancellationToken cancellationToken)
        {
            var portalConfig = await portalConfigRepository.GetByUserSubIdFirstOrDefaultAsync(request.UserSubId, cancellationToken);

            return await Result<ConfigsDto>.SuccessAsync(new ConfigsDto
            {
                PortalConfig = mapper.Map<PortalConfigDto>(portalConfig)
            });
        }
    }
}
