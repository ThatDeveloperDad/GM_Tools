using GameTools.Framework.Contexts;
using System.Security.Claims;

namespace GameTools.BlazorClient.Middleware
{
    internal static class ContextExtensions
    {
        public static GameToolsUser? GetUserContext(this HttpContext httpCtx)
        {
            GameToolsUser? userCtx = null;
            if(httpCtx == null)
            {
                return userCtx;
            }

            if (httpCtx.User == null)
            {
                return userCtx;
            }

            if ((httpCtx.User?.Identity?.IsAuthenticated ?? false)==false)
            {
                return userCtx;
            }

            var screenName = httpCtx.User!.Identity!.Name;
            var userId = httpCtx
                .User
                .Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?
                .Value;
            var roles = httpCtx
                .User
                .Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .ToList();

            if (userId != null)
            {
                userCtx = new GameToolsUser(userId)
                {
                    ScreenName = screenName
                };
                foreach (var roleClaim in roles)
                {
                    userCtx.AddUserRole(roleClaim.Value.ToString());
                }
            }

            return userCtx;
        }
    }
}
