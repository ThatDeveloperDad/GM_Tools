using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatDeveloperDad.Framework.Contexts
{
    public abstract class UserContextItem : ContextItemBase
    {
        protected UserContextItem()
        {
            UserId = new object();
        }

        public virtual object UserId {  get; protected init; }
    }
}
