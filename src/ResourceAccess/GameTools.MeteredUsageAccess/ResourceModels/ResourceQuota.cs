using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.MeteredUsageAccess.ResourceModels
{
    /// <summary>
    /// Represents the current state of a User's Quota consumption
    /// of a specific Metered Item.
    /// 
    /// It doesn't represent any calendar-bound or scheduled
    /// expiration / changes.  That calendar bound stuff is an
    /// attribute of the User's SUBSCRIPTION.
    /// </summary>
    public class ResourceQuota
    {
        public ResourceQuota() { }

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
