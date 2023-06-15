using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTelegram.Data.Entities
{
    [Table("tblAreas")]
    public class AreaEntity : BaseEntity<int>
    {
        [Required, StringLength(255)]
        public string Name { get; set; }
        public virtual ICollection<CityEntity> Cities { get; set; }
    }
}
