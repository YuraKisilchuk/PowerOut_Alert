namespace WebTelegram.Models.TelegramChats
{
    public class TelegramChatSearchResultViewModel
    {
        public List<TelegramChatItemViewModel> TelegramChats { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public int Total { get; set; }
    }
}
