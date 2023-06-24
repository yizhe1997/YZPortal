using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using YZPortal.API.Controllers.Pagination;
using YZPortal.Core.Domain.Contexts;
using YZPortal.FullStackCore.Models.Abstracts;

namespace YZPortal.API.Infrastructure.Mediatr
{
    public abstract class SearchRequestHandler<TRequest, TResponse> : BaseRequestHandler<TRequest, TResponse> where TRequest : SearchRequest<TResponse>
    {
        public SearchRequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
        {
        }

        #region Graph

        // For handling index response that query against runtime services 
        protected TResponse CreateIndexResponse<TModel>(SearchRequest<TResponse> request, List<TModel> results)
        {
            return Mapper.Map<TResponse>(new SearchModel<TModel>
            {
                Results = results,
                SearchString = request.SearchString,
                OrderBy = string.Join(",", request.OrderByArray),
                Select = string.Join(",", request.Select),
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                //TotalPages = (int)Math.Ceiling((double)usersMapped.Count / request.PageSize),
                TotalItems = results.Count
            });
        }

        #endregion

        // For handling index response that query against runtime services 
        protected TResponse CreateIndexResponse<TModel>(SearchRequest<TResponse> request)
        {
            List<TModel> results = new List<TModel>();
            int pageNumber = 0;
            int pageSize = 0;
            int totalPages = 0;
            int totalItems = 0;

            return Mapper.Map<TResponse>(new SearchModel<TModel>
            {
                Results = results,
                SearchString = request.SearchString,
                OrderBy = request.OrderBy,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalItems = totalItems
            });
        }

        protected async Task<TResponse> CreateIndexResponseAsync<TEntity, TModel>(SearchRequest<TResponse> request, IQueryable<TEntity> dbQuery, System.Linq.Expressions.Expression<Func<TEntity, bool>>? searchPredicate = null) where TEntity : class
        {
            if (!string.IsNullOrEmpty(request.SearchString) && searchPredicate != null)
            {
                dbQuery = dbQuery.Where(searchPredicate);
            }

            PaginatedList<TEntity> paginatedResults;
            List<TModel> results;
            int pageNumber;
            int pageSize;
            int totalPages;
            int totalItems;

            string orderByField;

            var columns = dbQuery.ElementType.GetProperties().Select(x => x.Name.ToLower());

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                orderByField = request.OrderBy.Split(" ").First();
            }
            else
            {
                request.OrderBy = orderByField = "Id";
            }

            // Make sure the property exists on the entity class before trying to sort by it
            if (columns.Contains(orderByField.ToLower()))
            {
                paginatedResults = await dbQuery.OrderBy(request.OrderBy).ToPaginatedListAsync(request);
                results = Mapper.Map<List<TModel>>(paginatedResults.Results);
                pageNumber = paginatedResults.PageNumber;
                pageSize = paginatedResults.PageSize;
                totalPages = paginatedResults.TotalPages;
                totalItems = paginatedResults.TotalItems;
            }
            // Check if the order by field is available on the view model
            else if (typeof(TModel).GetProperties().Select(x => x.Name.ToLower()).Contains(orderByField.ToLower()) == true)
            {
                var filteredResults = Mapper.Map<List<TModel>>(dbQuery.ToList());
                var internalResults = filteredResults.AsQueryable().OrderBy(request.OrderBy).ToPaginatedList(request);
                results = Mapper.Map<List<TModel>>(internalResults.Results);
                pageNumber = internalResults.PageNumber;
                pageSize = internalResults.PageSize;
                totalPages = internalResults.TotalPages;
                totalItems = internalResults.TotalItems;
            }
            // Otherwise use the default sorting (usually Id)
            else
            {
                paginatedResults = await dbQuery.ToPaginatedListAsync(request);
                results = Mapper.Map<List<TModel>>(paginatedResults.Results);
                pageNumber = paginatedResults.PageNumber;
                pageSize = paginatedResults.PageSize;
                totalPages = paginatedResults.TotalPages;
                totalItems = paginatedResults.TotalItems;
            }

            return Mapper.Map<TResponse>(new SearchModel<TModel>
            {
                Results = results,
                SearchString = request.SearchString,
                OrderBy = request.OrderBy,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalItems = totalItems,
                //Pages = results.Pages
            });
        }

        protected TResponse CreateIndexResponse<TEntity, TModel>(SearchRequest<TResponse> request, IQueryable<TEntity> dbQuery, System.Linq.Expressions.Expression<Func<TEntity, bool>> searchPredicate) where TEntity : class
        {
            if (!string.IsNullOrEmpty(request.SearchString) && searchPredicate != null)
            {
                dbQuery = dbQuery.Where(searchPredicate);
            }

            PaginatedList<TEntity> paginatedResults;
            List<TModel> results;
            int pageNumber;
            int pageSize;
            int totalPages;
            int totalItems;

            string orderByField;

            var columns = dbQuery.ElementType.GetProperties().Select(x => x.Name.ToLower());

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                orderByField = request.OrderBy.Split(" ").First();
            }
            else
            {
                request.OrderBy = orderByField = "Id";
            }

            // Make sure the property exists on the entity class before trying to sort by it
            if (columns.Contains(orderByField.ToLower()))
            {
                paginatedResults = dbQuery.OrderBy(request.OrderBy).ToPaginatedList(request);
                results = Mapper.Map<List<TModel>>(paginatedResults.Results);
                pageNumber = paginatedResults.PageNumber;
                pageSize = paginatedResults.PageSize;
                totalPages = paginatedResults.TotalPages;
                totalItems = paginatedResults.TotalItems;
            }
            // Check if the order by field is available on the view model
            else if (typeof(TModel).GetProperties().Select(x => x.Name.ToLower()).Contains(orderByField.ToLower()) == true)
            {
                var filteredResults = Mapper.Map<List<TModel>>(dbQuery.ToList());
                var internalResults = filteredResults.AsQueryable().OrderBy(request.OrderBy).ToPaginatedList(request);
                results = Mapper.Map<List<TModel>>(internalResults.Results);
                pageNumber = internalResults.PageNumber;
                pageSize = internalResults.PageSize;
                totalPages = internalResults.TotalPages;
                totalItems = internalResults.TotalItems;
            }
            // Otherwise use the default sorting (usually Id)
            else
            {
                paginatedResults = dbQuery.ToPaginatedList(request);
                results = Mapper.Map<List<TModel>>(paginatedResults.Results);
                pageNumber = paginatedResults.PageNumber;
                pageSize = paginatedResults.PageSize;
                totalPages = paginatedResults.TotalPages;
                totalItems = paginatedResults.TotalItems;
            }

            return Mapper.Map<TResponse>(new SearchModel<TModel>
            {
                Results = results,
                SearchString = request.SearchString,
                OrderBy = request.OrderBy,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalItems = totalItems,
                //Pages = results.Pages
            });
        }
    }
}
