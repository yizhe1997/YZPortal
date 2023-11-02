using Application.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddMediator();
            services.AddValidators();
        }

        private static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        private static void AddMediator(this IServiceCollection services)
        {
            // https://github.com/jbogard/MediatR/wiki/Migration-Guide-11.x-to-12.0
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            // Add MediatR behaviours
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ApplicationDbContextTransactionBehaviour<,>));
        }

        private static void AddValidators(this IServiceCollection services)
        {
            // Fluent Validation Ref: https://github.com/FluentValidation/FluentValidation/issues/1965
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
