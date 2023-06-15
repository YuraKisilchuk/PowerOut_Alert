namespace WebTelegram.Models.Cities
{
    public class CitySearchViewModel
    {
        public int Page { get; set; } = 1;
        public string Search { get; set; }
        public string Area { get; set; }
        public string Sort { get; set; }
        public int CountOnPage { get; set; } = 10;
    }
}
