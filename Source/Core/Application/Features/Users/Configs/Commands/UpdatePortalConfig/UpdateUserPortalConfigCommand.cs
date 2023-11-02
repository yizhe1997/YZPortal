using Application.Interfaces.Repositories;
using Application.Models;
using AutoMapper;
using MediatR;
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

    public class UpdatePortalConfigCommandHandler : IRequestHandler<UpdateUserPortalConfigCommand, Result>
    {
        private readonly IPortalConfigRepository _portalConfigRepository;
        private readonly IUnitOfWork<Guid> _unitOfWork;
        //private readonly IStringLocalizer<UpdatePortalConfigCommandHandler> _localizer;
        private readonly IMapper _mapper;

        public UpdatePortalConfigCommandHandler(
            IUnitOfWork<Guid> unitOfWork, 
            //IStringLocalizer<UpdatePortalConfigCommandHandler> localizer, 
            IMapper mapper, 
            IPortalConfigRepository portalConfigRepository)
        {
            _unitOfWork = unitOfWork;
            //_localizer = localizer;
            _mapper = mapper;
            _portalConfigRepository = portalConfigRepository;
        }

        public async Task<Result> Handle(UpdateUserPortalConfigCommand request, CancellationToken cancellationToken)
        {
            var portalConfig = await _portalConfigRepository.GetByUserSubIdFirstOrDefaultAsync(request.UserSubId);
            if (portalConfig != null)
            {
                _mapper.Map(request, portalConfig);

                await _unitOfWork.Commit(cancellationToken);

                return await Result.SuccessAsync();
            }
            else
            {
                return await Result.FailAsync("Portal Config Not Found.");
            }
        }
    }
}
