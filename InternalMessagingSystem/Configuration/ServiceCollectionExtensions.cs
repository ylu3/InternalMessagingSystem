using InternalMessagingSystem.Filters;
using InternalMessagingSystem.Services;

namespace InternalMessagingSystem.Configuration
{
    /// <summary>
    /// Extension methods for configuring internal messaging system services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds internal messaging system services to the specified <paramref name="services"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddInternalMessagingSystem(this IServiceCollection services)
        {
            // Register UserService as a singleton
            services.AddSingleton<IUserService, UserService>();

            // Register MessageService as a singleton
            services.AddSingleton<IMessageService, MessageService>();

            // Configure controllers with ApiExceptionFilter
            services.AddControllers(options =>
            {
                options.Filters.Add<ApiExceptionFilter>();
            });

            return services;
        }
    }
}
