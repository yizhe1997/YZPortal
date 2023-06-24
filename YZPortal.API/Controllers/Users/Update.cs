using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Error;
using YZPortal.FullStackCore.Models.Users;

namespace YZPortal.API.Controllers.Users
{
    public class Update
	{
		public class Request : IRequest<Model>
		{
			internal Guid SubjectId { get; set; }
		}
		public class Model : UserModel
        {
		}
		public class RequestHandler : BaseRequestHandler<Request, Model>
		{
			public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
			{
			}
			public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
			{
				var user = await Database.Users.FirstOrDefaultAsync(u => u.SubjectIdentifier == request.SubjectId);
				if (user == null)
					throw new RestException(HttpStatusCode.NotFound, "User not found.");

				// Map input to user and save the changes
				user.SubjectIdentifier = CurrentContext.NameIdentifier;
				user.DisplayName = CurrentContext.DisplayName;
				user.AuthTime = CurrentContext.AuthTime;
				user.LastidpAccessToken = CurrentContext.IdpAccessToken;
				user.AuthExpireTime = CurrentContext.AuthExpireTime;
				user.AuthClassRef = CurrentContext.AuthClassRef;
				user.IpAddress = CurrentContext.IpAddress;
				user.Email = CurrentContext.Email;
                user.UpdatedDate = DateTime.UtcNow;
                user.UpdatedBy = CurrentContext.NameIdentifier.ToString();

				await Database.SaveChangesAsync();

				// Mapping
				var result = Mapper.Map<Model>(user);

				return result;
			}
		}
	}
}
