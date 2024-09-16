using GameTools.Framework.Contexts;
using System.Security.Claims;

namespace GameTools.BlazorClient.Middleware
{
    public class AppContextMiddleware
    {
        private readonly RequestDelegate _next;

        public AppContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext httpCtx,
            ContextContainer appCtx)
        {
            GameToolsUser? userCtx = appCtx.GetCurrentUser();
            if (userCtx == null)
            {
                // Build the userCtx from the Principal on HttpCtx.
                if(httpCtx.User?.Identity?.IsAuthenticated??false)
                {
                    userCtx = httpCtx.GetUserContext();
                    if(userCtx != null)
                    {
                        appCtx.SetUserContext(userCtx);
                    }
                }
            }
            await _next(httpCtx);
        }
    }
}
