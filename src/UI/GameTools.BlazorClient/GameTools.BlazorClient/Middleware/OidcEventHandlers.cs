using GameTools.Framework.Contexts;
using GameTools.UserAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Linq.Dynamic.Core;
using System.Security.Claims;

namespace GameTools.BlazorClient.Middleware
{
	public static class OidcEventHandlers
	{
		public static async Task OnRedirectToIdP(RedirectContext ctx)
		{
			await Task.Yield();
		}

		public static async Task OnAuthenticationfailed(AuthenticationFailedContext ctx)
		{
			await Task.Yield();
		}

		public static async Task OnSignedOutCallbackRedirect(RemoteSignOutContext ctx)
		{
			ctx.HttpContext.Response.Redirect(ctx.Options.SignedOutRedirectUri);
			ctx.HandleResponse();
			await Task.Yield();
		}

		public static async Task OnTicketReceived(TicketReceivedContext ctx)
		{
			if (ctx.Principal != null)
			{
				if (ctx.Principal.Identity is ClaimsIdentity identity)
				{
					var colClaims = await ctx.Principal.Claims.ToDynamicListAsync();
					var IdentityProvider = colClaims.FirstOrDefault(
						c => c.Type == "http://schemas.microsoft.com/identity/claims/identityprovider")?.Value;
					var Objectidentifier = colClaims.FirstOrDefault(
						c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
					var EmailAddress = colClaims.FirstOrDefault(
						c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
					var FirstName = colClaims.FirstOrDefault(
						c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")?.Value;
					var LastName = colClaims.FirstOrDefault(
						c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")?.Value;
					var AzureB2CFlow = colClaims.FirstOrDefault(
						c => c.Type == "http://schemas.microsoft.com/claims/authnclassreference")?.Value;
					var auth_time = colClaims.FirstOrDefault(
						c => c.Type == "auth_time")?.Value;
					var DisplayName = colClaims.FirstOrDefault(
						c => c.Type == "name")?.Value;
					var idp_access_token = colClaims.FirstOrDefault(
						c => c.Type == "idp_access_token")?.Value;

					// Check to see if the custom Role claims have been added.
					if (ctx.Principal.HasClaim(c => c.Type == "AppRolesExist") == false)
					{
						if (Objectidentifier != null)
						{
							IUserAccess userAccess = ctx
							.HttpContext
							.RequestServices
							.GetRequiredService<IUserAccess>();

                            

                            List<string>? groups =
								await userAccess?
								.LoadUserPermissionGroupsAsync(Objectidentifier)
								?? new List<string>();

							var newClaims = new List<Claim>();
							foreach (var group in groups)
							{
								var claim = new Claim(ClaimTypes.Role, group);
								newClaims.Add(claim);
								
							}
							if (newClaims.Any())
							{
								newClaims.Add(
									new Claim(type: "AppRolesExist", value: "true"));
							}

							var appIdentity = new ClaimsIdentity(newClaims);

							ctx.Principal.AddIdentity(appIdentity);
							
                        }
					}
				}
			}
			await Task.Yield();
		}
	}
}
