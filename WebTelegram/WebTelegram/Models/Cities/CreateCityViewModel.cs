using System.ComponentModel.DataAnnotations;

namespace WebTelegram.Models.Cities
{
    public class CreateCityViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int AreaId { get; set; }
    }
}
