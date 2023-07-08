using AutoMapper;
using YZPortal.API.Controllers.ControllerRequests.Indexes;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.FullStackCore.Models.Abstracts;

namespace YZPortal.API.Infrastructure.Mediatr
{
    public abstract class SearchRequestHandler<TRequest, TResponse> : BaseRequestHandler<TRequest, TResponse> where TRequest : SearchRequest<TResponse>
    {
        public SearchRequestHandler(DatabaseService dbService, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbService, mapper, httpContext, userAccessor)
        {
        }

        #region Graph

        // For handling index response that query against runtime services, move this pls...
        protected TResponse CreateIndexResponse<TModel>(SearchRequest<TResponse> request, List<TModel> results)
        {
            return Mapper.Map<TResponse>(new SearchModel<TModel>
            {
                Results = results,
                SearchString = request.SearchString,
                OrderBy = request.OrderBy,
                Select = request.Select,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                // TotalPages and TotalItems cant be mapped atm due to limitations
                TotalPages = 0,
                TotalItems = 0
            });
        }

        #endregion
    }
}
