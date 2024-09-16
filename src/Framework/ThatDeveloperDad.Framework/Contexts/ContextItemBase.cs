using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatDeveloperDad.Framework.Contexts
{
    /// <summary>
    /// Defines the smallest common set of properties and behaviors that an
    /// item that could be added to a ContextBase's dictionary must have.
    /// </summary>
    public abstract class ContextItemBase
    {
        public abstract string ContextKey { get; }

        protected ContextItemBase() { }
    }
}
