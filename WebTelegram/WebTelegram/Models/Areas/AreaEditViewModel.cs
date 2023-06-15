using System.ComponentModel.DataAnnotations;

namespace WebTelegram.Models.Areas
{
    public class AreaEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
