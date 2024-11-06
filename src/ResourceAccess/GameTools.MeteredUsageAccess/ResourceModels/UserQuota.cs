using GameTools.Framework.Concepts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.MeteredUsageAccess.ResourceModels
{
    public class UserQuota
    {
        public UserQuota()
        {
            UserId = string.Empty;
            Storage = new ResourceQuota()
            {
                MeteredResource=MeteredResourceKind.NpcStorage
            };

            AiGenerations = new ResourceQuota()
            {
                MeteredResource = MeteredResourceKind.NpcAiDetail
            };
        }

        public string UserId { get; set; }

        public ResourceQuota? GetQuota(int quotaId)
        {
            // This is a little gross, but is a holdover from
            // when Quotas were stored relationally instead of
            // as complex properties in Table Storage.
            if (Storage.QuotaId == quotaId)
            {
                return Storage;
            }
            else if (AiGenerations.QuotaId == quotaId)
            {
                return AiGenerations;
            }
            else
            {
                return null;
            }
        }

        public ResourceQuota Storage { get; set; }

        public ResourceQuota AiGenerations { get; set; }
    }
}
