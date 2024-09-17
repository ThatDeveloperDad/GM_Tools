using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.MeteredUsageAccess.SqlServer.Context.SqlModels
{
    [Table("QuotaTemplates", Schema="usage")]
    internal class QuotaTemplateSqlModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(25)]
        public string ResourceKind { get; set; }
        
        [Required, MaxLength(25)]
        public string ResourceCategory { get; set; }
        
        [Required]
        public bool IsPeriodic { get; set; }

        [Required]
        public int UnitsGranted { get; set; }

        [Required]
        public int PeriodDays { get; set; }

        [Required, Column("CreatedDate")]
        public DateTime CreatedDateUtc { get; set; }

        [Column("UpdatedDate")]
        public DateTime? UpdatedDateUtc { get; set; }

        [Column("DeletedDate")]
        public DateTime? DeletedDateUtc { get; set; }
    }
}
