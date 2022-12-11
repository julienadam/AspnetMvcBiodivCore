using AspNetBiodiv.Core.Web.Models;
using AspNetBiodiv.Core.Web.Services.Especes;
using AspNetBiodiv.Core.Web.Services.Observations;
using Microsoft.AspNetCore.Mvc;

namespace AspNetBiodiv.Core.Web.Controllers
{
    [Route("especes")]
    public class EspecesController : Controller
    {
        private readonly ITaxonomie taxonomie;
        private readonly IObservations observations;

        public EspecesController(ITaxonomie taxonomie, IObservations observations)
        {
            this.taxonomie = taxonomie ?? throw new ArgumentNullException(nameof(taxonomie));
            this.observations = observations ?? throw new ArgumentNullException(nameof(observations));
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        
        [Route("{id:int}")]
        public IActionResult Detail(int id)
        {
            var espece = taxonomie.RechercherParId(id);
            if (espece == null)
            {
                return NotFound();
            }

            var observationsEspece = observations.GetObservationsForEspece(espece.Id);

            return View(MapEspeceViewModel(espece, observationsEspece));
        }

        [Route("{nomSci}")]
        public IActionResult Detail(string nomSci)
        {
            var espece = taxonomie.RechercherParNomScientifique(nomSci);
            if (espece == null)
            {
                return NotFound();
            }
            
            var observationsEspece = observations.GetObservationsForEspece(espece.Id);

            return View(MapEspeceViewModel(espece, observationsEspece));
        }

        [Route("tags/{tag}")]
        public IActionResult Tags(string tag)
        {
            var especes = taxonomie
                .RechercherParTag(tag)
                .Select(e => MapEspeceViewModel(e))
                .ToList();
            ViewData["Title"] = $"Recherche par tag {tag}"; 
            return View("Resultats", especes);
        }

        private static EspeceViewModel MapEspeceViewModel(Espece e, IEnumerable<Observation>? observationsEspece = null) =>
            new()
            {
                Id = e.Id,
                NomScientifique = e.NomScientifique,
                UrlIconeHabitat = $"/img/habitat/{e.Habitat.ToString().ToLowerInvariant()}.png",
                HabitatAlt = e.Habitat.ToString(),
                UrlIconePresence = $"/img/presence/{char.ToLower((char)(int)e.Presence)}.svg",
                PresenceAlt = e.Presence.ToString(),
                CodeInpn = e.IdInpn,
                Observations = observationsEspece?.Select(ObservationViewModel.FromObservation) 
                               ?? Enumerable.Empty<ObservationViewModel>()
            };

        [Route("{year:int}/{month:int}")]
        public IActionResult ByYearMonth(int year, int month)
        {
            if (month is > 12 or < 1)
            {
                return BadRequest();
            }

            var especes = taxonomie
                .RechercherParMois(year, month)
                .Select(e => MapEspeceViewModel(e))
                .ToList();
            ViewData["Title"] = $"Recherche pour {year}/{month}";
            return View("Resultats", especes);
        }
    }
}
