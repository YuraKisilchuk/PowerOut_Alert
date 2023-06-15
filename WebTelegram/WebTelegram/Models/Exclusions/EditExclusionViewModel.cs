using System.ComponentModel.DataAnnotations;

namespace WebTelegram.Models.Exclusions
{
    public class EditExclusionViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string BeginExclusion { get; set; }        
        [Required]
        public string EndExclusion { get; set; }
        [Required]
        public int CityId { get; set; }
    }
}
