using Microsoft.Extensions.Options;

namespace GameTools.BlazorClient.Middleware
{
	internal static class AuthorizationPolicies
	{
		public static IServiceCollection SetAuthorizationPolicies(this IServiceCollection services)
		{
			services.AddAuthorization(options =>
			{
				options.AddPolicy(AuthZConstants.pol_KnownUsers,
					policy => policy.RequireRole(AuthZConstants.role_KnownUser));

				options.AddPolicy(AuthZConstants.pol_PaidUsers,
					policy => policy.RequireRole(AuthZConstants.role_PaidUser));

			});

			return services;
		}
	}
}
