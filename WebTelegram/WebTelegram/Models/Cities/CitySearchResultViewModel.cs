namespace WebTelegram.Models.Cities
{
    public class CitySearchResultViewModel
    {
        public List<CityItemViewModel> Cities { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public int Total { get; set; }
    }
}
