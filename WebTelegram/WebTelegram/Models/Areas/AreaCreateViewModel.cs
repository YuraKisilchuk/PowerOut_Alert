using System.ComponentModel.DataAnnotations;

namespace WebTelegram.Models.Areas
{
    public class AreaCreateViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
