using System.Security.Claims;
using System.Security.Principal;

namespace GameTools.BlazorClient.Middleware
{
	internal static class UserHelper
	{
		public static string? UserId(this IIdentity identity)
		{
			string? userID = null;

			userID = identity.GetClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

			return userID;
		}

		public static string? Email(this IIdentity identity)
		{
			string? email = null;

			email = identity.GetClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

			return email;
		}

		private static string? GetClaim(this IIdentity identity, string claimType)
		{
			string? claimValue = null;
			if(identity is ClaimsIdentity cid)
			{
				claimValue = cid.FindFirst(claimType)?.Value;
			}

			return claimValue;
		}
	}
}
