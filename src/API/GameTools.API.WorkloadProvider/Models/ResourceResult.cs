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
		public QuotaContainer? UpdatedQuotas { get; set; }

		public T? Result { get; set; }
	}
}
