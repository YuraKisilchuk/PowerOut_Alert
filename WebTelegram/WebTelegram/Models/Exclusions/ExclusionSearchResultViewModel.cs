namespace WebTelegram.Models.Exclusions
{
    public class ExclusionSearchResultViewModel
    {
        public List<ExclusionItemViewModel> Exclusions { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public int Total { get; set; }
    }
}
