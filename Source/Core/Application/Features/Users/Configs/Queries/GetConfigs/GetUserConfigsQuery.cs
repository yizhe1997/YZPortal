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

    public class GetConfigsQueryHandler : IRequestHandler<GetUserConfigsQuery, Result<ConfigsDto>>
    {
        private readonly IPortalConfigRepository _portalConfigRepository;
        private readonly IMapper _mapper;

        public GetConfigsQueryHandler(
            IMapper mapper,
            IPortalConfigRepository portalConfigRepository)
        {
            _mapper = mapper;
            _portalConfigRepository = portalConfigRepository;
        }

        public async Task<Result<ConfigsDto>> Handle(GetUserConfigsQuery request, CancellationToken cancellationToken)
        {
            var portalConfig = await _portalConfigRepository.GetByUserSubIdFirstOrDefaultAsync(request.UserSubId);

            return await Result<ConfigsDto>.SuccessAsync(new ConfigsDto
            {
                PortalConfig = _mapper.Map<PortalConfigDto>(portalConfig)
            });
        }
    }
}
