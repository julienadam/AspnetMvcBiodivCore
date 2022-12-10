using AspNetBiodiv.Core.Web.Models;
using AspNetBiodiv.Core.Web.Services.Especes;
using Microsoft.AspNetCore.Mvc;

namespace AspNetBiodiv.Core.Web.Controllers
{
    [Route("especes")]
    public class EspecesController : Controller
    {
        private readonly ITaxonomie taxonomie;

        public EspecesController(ITaxonomie taxonomie)
        {
            this.taxonomie = taxonomie ?? throw new ArgumentNullException(nameof(taxonomie));
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

            return View(MapEspeceViewModel(espece));
        }

        [Route("{nomSci}")]
        public IActionResult Detail(string nomSci)
        {
            var espece = taxonomie.RechercherParNomScientifique(nomSci);
            if (espece == null)
            {
                return NotFound();
            }

            return View(MapEspeceViewModel(espece));
        }

        [Route("tags/{tag}")]
        public IActionResult Tags(string tag)
        {
            var especes = taxonomie
                .RechercherParTag(tag)
                .Select(MapEspeceViewModel)
                .ToList();
            ViewData["Title"] = $"Recherche par tag {tag}"; 
            return View("Resultats", especes);
        }

        private static EspeceViewModel MapEspeceViewModel(Espece e) =>
            new()
            {
                Id = e.Id,
                NomScientifique = e.NomScientifique,
                UrlIconeHabitat = $"/img/habitat/{e.Habitat.ToString().ToLowerInvariant()}.png",
                HabitatAlt = e.Habitat.ToString(),
                UrlIconePresence = $"/img/presence/{char.ToLower((char)(int)e.Presence)}.svg",
                PresenceAlt = e.Presence.ToString(),
                UrlInpn = $"https://inpn.mnhn.fr/espece/cd_nom/{e.IdInpn}"
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
                .Select(MapEspeceViewModel)
                .ToList();
            ViewData["Title"] = $"Recherche pour {year}/{month}";
            return View("Resultats", especes);
        }
    }
}
