using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.MeteredUsageAccess.SqlServer.Context.SqlModels
{
    [Table("TokenConsumption", Schema="usage")]
    internal class TokenConsumptionSqlModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required, Column("InferenceTime")]
        public DateTime InferenceTimeUtc { get; set; }

        [Required]
        public string FunctionName { get; set; }

        [Required] 
        public int PromptTokens { get; set; }

        [Required]
        public int CompletionTokens { get; set; }

        [Required]
        public string modelId { get; set; }
    }
}
