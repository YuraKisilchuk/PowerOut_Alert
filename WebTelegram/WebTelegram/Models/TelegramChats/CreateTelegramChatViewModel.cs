using System.ComponentModel.DataAnnotations;

namespace WebTelegram.Models.TelegramChats
{
    public class CreateTelegramChatViewModel
    {
        [Required]
        public long ChatId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
    }
}
