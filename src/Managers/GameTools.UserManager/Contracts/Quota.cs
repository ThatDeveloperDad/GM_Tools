using GameTools.Framework.Concepts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.UserManager.Contracts
{
	public class Quota
	{
		/// <summary>
		/// Identifies the general kind of resource that has this quota
		/// 
		/// i.e.:  Storage or AiTokens
		/// </summary>
		public MeteredResourceKind MeteredResource { get; set; }

		/// <summary>
		/// Provides the specific name of the Resource that has the quota
		/// 
		/// i.e.:  NpcStorage, NpcAiGeneration
		/// </summary>
		public string ResourceName => MeteredResource.ToString();

		/// <summary>
		/// Describes the amount of the Specific Resource that the 
		/// user can consume.
		/// </summary>
		public int Budget { get; set; }

		/// <summary>
		/// Describes the amount of the Specific Resource that the user
		/// HAS consumed.
		/// </summary>
		public int Consumption { get; set; }
	}
}
