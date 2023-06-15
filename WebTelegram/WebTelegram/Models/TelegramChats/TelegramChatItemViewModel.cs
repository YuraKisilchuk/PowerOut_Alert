using System.ComponentModel.DataAnnotations;

namespace WebTelegram.Models.TelegramChats
{
    public class TelegramChatItemViewModel
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
    }
}
