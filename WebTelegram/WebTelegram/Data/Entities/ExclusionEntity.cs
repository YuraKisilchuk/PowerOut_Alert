using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTelegram.Data.Entities
{
    [Table("tblExclusions")]
    public class ExclusionEntity : BaseEntity<int>
    {
        [Required, StringLength(255)]
        public string Name { get; set; }
        [StringLength(255)]
        public DateTime BeginExclusion { get; set; }
        [Required, StringLength(255)]
        public DateTime EndExclusion { get; set; }
        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual CityEntity City { get; set; }

        public virtual ICollection<ExclusionEntity> Exclusions { get; set; }
    }
}
