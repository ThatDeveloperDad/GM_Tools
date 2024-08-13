using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.NPCAccess.SqlServer.Context.SqlModels
{
    [Table("Npcs", Schema ="userdata")]
    internal class NpcRowModel
    {
        public NpcRowModel()
        {
            SpeciesName = string.Empty;
            VocationName = string.Empty;
            CharacterDetails = string.Empty;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NpcId { get; set; }
        public int UserId { get; set; }
        public string SpeciesName { get; set; }
        public string VocationName { get; set; }
        public string? CharacterName { get; set; }
        public string CharacterDetails { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
