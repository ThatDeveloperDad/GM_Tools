using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatDeveloperDad.Framework.Contexts
{
    /// <summary>
    /// Defines the base attributes and behaviors of an object that can carry
    /// contextual information about the execution of a Workload, Process, or
    /// method.
    /// 
    /// Usually, this "context" could be considered akin to Metadata.
    /// </summary>
    public abstract class ContextBase
    {
        private readonly Dictionary<string, object?> _contextItems;

        protected ContextBase()
        {
            _contextItems = new Dictionary<string, object?>();
        }

        public virtual void AddItem(string key, object? value)
        {
            if (_contextItems.ContainsKey(key) == false)
            {
                _contextItems.Add(key, value);
            }
            else
            {
                throw new ArgumentException($"Cannot add {key} to the Context.  That value has already been assigned. ");
            }
        }

        public virtual void UpdateItem(string key, object? value)
        {
            if (_contextItems.ContainsKey(key))
            {
                _contextItems[key] = value;
            }
            else
            {
                throw new ArgumentException($"Cannot Update the context item at key {key}.  It could not be found on the context object.");
            }
        }

        public virtual object? RetrieveItem(string key)
        {
            _contextItems.TryGetValue(key, out object? val);
            return val;
        }
    }
}
