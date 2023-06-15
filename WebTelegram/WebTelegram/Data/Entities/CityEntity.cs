using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTelegram.Data.Entities
{
    [Table("tblCities")]
    public class CityEntity : BaseEntity<int>
    {
        [Required, StringLength(255)]
        public string Name { get; set; }
        [ForeignKey("Area")]
        public int AreaId { get; set; }
        public virtual AreaEntity Area { get; set; }
        public virtual ICollection<ExclusionEntity> Exclusions { get; set; }
    }
}
