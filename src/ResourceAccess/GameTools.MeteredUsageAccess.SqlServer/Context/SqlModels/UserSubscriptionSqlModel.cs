using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.MeteredUsageAccess.SqlServer.Context.SqlModels
{
    [Table("UserSubscriptions", Schema="usage")]
    internal class UserSubscriptionSqlModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required, Column("PeriodStart")]
        public DateTime PeriodStartUtc { get; set; }

        [Required, Column("PeriodEnd")]
        public DateTime PeriodEndUtc { get; set; }

        [Required, Column("CreatedDate")]
        public DateTime CreatedDateUtc { get; set; }

        [Column("UpdatedDate")]
        public DateTime? UpdatedDateUtc { get; set; }

        [Column("UnsubscribedDate")]
        public DateTime? UnsubscribedDateUtc { get; set; }
    }
}
