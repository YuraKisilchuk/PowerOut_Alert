namespace WebTelegram.Models.Areas
{
    public class AreaSearchViewModel
    {
        public int Page { get; set; } = 1;
        public string Search { get; set; }
        public int CountOnPage { get; set; } = 10;
    }
}
