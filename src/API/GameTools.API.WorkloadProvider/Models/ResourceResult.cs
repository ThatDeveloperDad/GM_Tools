using GameTools.UserManager.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.API.WorkloadProvider.Models
{
	/// <summary>
	/// Use this as the OpResult Payload when an operation changes a
	/// user's quotas for one or more Limited Resources.
	/// 
	/// The updated QuotaContainer should be placed in the UpdatedQuotas 
	/// property, the actual Application Type that is expected should go 
	/// into the Result property.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ResourceResult<T> where T : class
	{
		/// <summary>
		/// Carries the most recently updated Quotas for the user that requested
		/// the operation for which this is a result.
		/// </summary>
		public QuotaContainer? UpdatedQuotas { get; set; }

		/// <summary>
		/// Carries the result of the operation.
		/// </summary>
		public T? Result { get; set; }
	}
}
