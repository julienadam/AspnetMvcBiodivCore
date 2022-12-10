using Microsoft.AspNetCore.Mvc;

namespace AspNetBiodiv.Core.Web.Controllers
{
    public class EspecesController : Controller
    {
        public IActionResult Index()
        {
            return Content("Formulaire de recherche");
        }

        public IActionResult Create()
        {
            return Content("Formulaire de saisie d'une nouvelle espèce");
        }

        [HttpPost]
        public IActionResult Create(string data)
        {
            return Content($"Enregistrement d'une nouvelle espèce à partir des données {data}");
        }

        public IActionResult Detail(int? id, string? nomSci)
        {
            if (id != null)
            {
                return Content($"Détails sur l'espèce {id}");
            }

            if(nomSci != null)
            {
                return Content($"Détails sur l'espèce {nomSci}");
            }

            return base.BadRequest("Soit un id, soit un nom scientifique !");
        }

        public IActionResult Tags(string tag)
        {
            return Content($"Recherche par tag : {tag}");
        }
    }
}
