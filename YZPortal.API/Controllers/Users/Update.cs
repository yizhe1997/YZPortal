using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Error;

namespace YZPortal.API.Controllers.Users
{
    public class Update
	{
		public class Request : IRequest<Model>
		{
			internal Guid Id { get; set; }
			public string? Name { get; set; }
			public string? UserName { get; set; }
		}
		public class Validator : AbstractValidator<Request>
		{
			public Validator()
			{
				RuleFor(x => x.UserName).NotNull().NotEmpty().EmailAddress();
				RuleFor(x => x.Name).NotNull().NotEmpty();
			}
		}
		public class Model : UserViewModel
		{
		}
		public class RequestHandler : BaseRequestHandler<Request, Model>
		{
			public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
			{
			}
			public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
			{
				var user = await Database.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
				if (user == null)
					throw new RestException(HttpStatusCode.NotFound, "User not found.");

				// Map input to user and save the changes
				Mapper.Map(request, user);
				await Database.SaveChangesAsync();

				// Mapping
				var result = Mapper.Map<Model>(user);

				return result;
			}
		}
	}
}
