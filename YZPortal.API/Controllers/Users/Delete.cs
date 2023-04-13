﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YZPortal.API.Controllers.Users;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Error;

namespace YZPortal.API.Controllers.Users
{
	public class Delete
	{
		public class Request : IRequest<Model>
		{
			internal Guid Id { get; set; }
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

				// Forbid deletion of admin user
				if (user.Admin == true)
					throw new RestException(HttpStatusCode.NotFound, "Admin user not allowed for deletion.");

				// Remove from users table if record exit
				Database.Users.Remove(user);
				await Database.SaveChangesAsync();

				return Mapper.Map<Model>(user);
			}
		}
	}
}