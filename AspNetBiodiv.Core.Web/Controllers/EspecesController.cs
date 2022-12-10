using AspNetBiodiv.Core.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetBiodiv.Core.Web.Controllers
{
    [Route("especes")]
    public class EspecesController : Controller
    {
        private readonly IRechercheEspecesService serviceRecherche;

        public EspecesController(IRechercheEspecesService serviceRecherche)
        {
            this.serviceRecherche = serviceRecherche ?? throw new ArgumentNullException(nameof(serviceRecherche));
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        
        [Route("{id:int}")]
        public IActionResult Detail(int id)
        {
            var espece = serviceRecherche.RechercherParId(id);
            if (espece == null)
            {
                return NotFound();
            }      
            
            return View(espece);
        }

        [Route("{nomSci}")]
        public IActionResult Detail(string nomSci)
        {
            var espece = serviceRecherche.RechercherParNomScientifique(nomSci);
            if (espece == null)
            {
                return NotFound();
            }

            return View(espece);
        }

        [Route("tags/{tag}")]
        public IActionResult Tags(string tag)
        {
            var especes = serviceRecherche.RechercherParTag(tag).ToList();
            ViewData["Title"] = $"Recherche par tag {tag}"; 
            return View("Resultats", especes);
        }
        
        [Route("{year:int}/{month:int}")]
        public IActionResult ByYearMonth(int year, int month)
        {
            if (month is > 12 or < 1)
            {
                return BadRequest();
            }

            var especes = serviceRecherche.RechercherParMois(year, month).ToList();
            ViewData["Title"] = $"Recherche pour {year}/{month}";
            return View("Resultats", especes);
        }
    }
}
