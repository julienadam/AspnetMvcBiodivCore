namespace AspNetBiodiv.Core.Web.Models
{
    public class EspeceViewModel
    {
        public int Id { get; set; }
        public string NomScientifique { get; set; }
        public string UrlIconeHabitat { get; set; }
        public string UrlIconePresence { get; set; }
        public string HabitatAlt { get; set; }
        public string PresenceAlt { get; set; }
        public IEnumerable<ObservationViewModel> Observations { get; set; } = new List<ObservationViewModel>();
        public int CodeInpn { get; set; }
    }
}
