using System.ComponentModel.DataAnnotations;

namespace WebTelegram.Models.Cities
{
    public class EditCityViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int AreaId { get; set; }
    }
}
