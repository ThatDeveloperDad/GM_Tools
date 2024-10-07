using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.MeteredUsageAccess.SqlServer.Context.SqlModels
{
    [Table("UserQuotas", Schema="usage")]
    internal class UserQuotaSqlModel
    {
        public UserQuotaSqlModel()
        {
            UserId = string.Empty;
            CreatedDateUtc = DateTime.UtcNow;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string UserId { get; set; }

        [Required]
        public int QuotaTemplateId { get; set; }

        [Required]
        public int CurrentBudget { get; set; }

        [Required]
        public int ConsumedBudget { get; set; }

        [Required, Column("CreatedDate")]
        public DateTime CreatedDateUtc { get; set; }

        [Column("UpdatedDate")]
        public DateTime? UpdatedDateUtc { get; set; }
    }
}
