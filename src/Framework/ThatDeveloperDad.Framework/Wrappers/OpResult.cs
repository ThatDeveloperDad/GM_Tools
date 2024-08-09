using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatDeveloperDad.Framework.Wrappers
{
    /// <summary>
    /// Represents the result of an operation invoked on external code.
    /// The externality of that code does not have to be a physical separation,
    /// use of this pattern is valid for calling from component to component
    /// within the same process space.
    /// </summary>
    public class OpResult
    {
        private Dictionary<Guid, string> _errors = new Dictionary<Guid, string>();

        public OpResult()
        {
            
        }

        public bool WasSuccessful => (_errors.Any() == false);

        public ReadOnlyDictionary<Guid, string> Errors => _errors.AsReadOnly<Guid, string>();

        public void AddError(Guid errorId, string errorMessage)
        {
            _errors[errorId] = errorMessage;
        }

    }

    /// <summary>
    /// Extends the basic OpResult to allow instances to carry the expected value
    /// in the case of a operation with a return value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OpResult<T> : OpResult
    {
        public OpResult(T result)
        {
            Result = result;
        }

        public OpResult()
        {
            Result = default(T);
        }

        public T? Result { get; set; }
    }
}
