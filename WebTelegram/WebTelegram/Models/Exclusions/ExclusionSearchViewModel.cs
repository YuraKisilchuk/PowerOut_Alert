namespace WebTelegram.Models.Exclusions
{
    public class ExclusionSearchViewModel
    {
        public int Page { get; set; } = 1;
        public string Search { get; set; }
        public string City { get; set; }
        public string Sort { get; set; }
        public int CountOnPage { get; set; } = 10;
    }
}
