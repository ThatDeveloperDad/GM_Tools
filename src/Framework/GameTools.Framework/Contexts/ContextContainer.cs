using ThatDeveloperDad.Framework.Contexts;

namespace GameTools.Framework.Contexts
{
    /// <summary>
    /// Provides an object that we can insantiate in whatever application host
    /// we need that can hold the specific kinds of Contextual information we
    /// want available to the other parts of the application.
    /// </summary>
    public sealed class ContextContainer : ContextBase
    {

        public void SetUserContext(GameToolsUser ctx)
        {
            var existing = GetCurrentUser();
            if(existing == null)
            {
                AddItem(ctx.ContextKey, ctx);
            }
            else
            {
                UpdateItem(ctx.ContextKey, ctx);
            }
        }

        public GameToolsUser? GetCurrentUser()
        {
            GameToolsUser? ctx = RetrieveItem(GameToolsUser.ItemTypeId) as GameToolsUser;
            return ctx;
        }
    }
}
