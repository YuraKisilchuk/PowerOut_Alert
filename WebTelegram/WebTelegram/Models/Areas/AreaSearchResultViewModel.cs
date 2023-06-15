namespace WebTelegram.Models.Areas
{
    public class AreaSearchResultViewModel
    {
        public List<AreaItemViewModel> Areas { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public int Total { get; set; }
    }
}
