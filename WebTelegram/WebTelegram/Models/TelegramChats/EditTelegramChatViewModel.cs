using System.ComponentModel.DataAnnotations;

namespace WebTelegram.Models.TelegramChats
{
    public class EditTelegramChatViewModel
    {
        [Required]
        public int Id { get; set; }
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
