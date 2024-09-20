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

        public virtual void CopyErrorsTo(ref OpResult target)
        {
            foreach(var error in this._errors)
            {
                target.AddError(error.Key, error.Value);
            }
        }

        public string AggregateErrors()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Errors occurred while processing the activity");
            foreach(var error in this._errors)
            {
                sb.AppendLine(error.Value);
            }
            return sb.ToString();
        }

        //public bool ShouldRetry { get; set; }
        //public int RetryCount { get; set; }
        //public int RetryLimit { get; set; }
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
            Payload = result;
        }

        public OpResult()
        {
            Payload = default(T);
        }

        public T? Payload { get; set; }

        public virtual void CopyErrorsTo<U>(ref OpResult<U> target)
        {
            foreach(var error in Errors)
            {
                target.AddError(error.Key, error.Value);
            }
        }
    }
}
