using Microsoft.AspNetCore.Mvc;

namespace AspNetBiodiv.Core.Web.Controllers
{
    [Route("especes")]
    public class EspecesController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return Content("Formulaire de recherche");
        }
        
        [Route("{id:int}")]
        public IActionResult Detail(int id)
        {
            return Content($"Détails sur l'espèce {id}");
        }

        [Route("{nomSci}")]
        public IActionResult Detail(string nomSci)
        {
            return Content($"Détails sur l'espèce {nomSci}");
        }

        [Route("tags/{tag}")]
        public IActionResult Tags(string tag)
        {
            return Content($"Recherche par tag : {tag}");
        }

        [Route("{year:int}/{month:int}")]
        public IActionResult Tags(int year, int month)
        {
            if (month is > 12 or < 1)
            {
                return NotFound();
            }

            return Content($"Recherche par mois : {month} de l'année {year}");
        }
    }
}
