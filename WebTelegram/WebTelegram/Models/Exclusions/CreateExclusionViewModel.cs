using System.ComponentModel.DataAnnotations;

namespace WebTelegram.Models.Exclusions
{
    public class CreateExclusionViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        public string BeginExclusion { get; set; }
        [Required]
        public string EndExclusion { get; set; }
    }
}
