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
            return Content("Formulaire de recherche");
        }
        
        [Route("{id:int}")]
        public IActionResult Detail(int id)
        {
            var espece = serviceRecherche.RechercherParId(id);
            if (espece == null)
            {
                return NotFound();
            }      
            
            return Content($"Détails sur l'espèce {espece.Id} : nom scientifique {espece.NomScientifique}");
        }

        [Route("{nomSci}")]
        public IActionResult Detail(string nomSci)
        {
            var espece = serviceRecherche.RechercherParNomScientifique(nomSci);
            if (espece == null)
            {
                return NotFound();
            }

            return Content($"Détails sur l'espèce {espece.Id} : nom scientifique {espece.NomScientifique}");
        }

        [Route("tags/{tag}")]
        public IActionResult Tags(string tag)
        {
            var especes = serviceRecherche.RechercherParTag(tag).ToList();
            return Content($"{especes.Count} trouvées\n{FormatEspeces(especes)}");
        }

        private static string FormatEspeces(IEnumerable<Espece> especes) =>
            string.Join("\n", especes.Select(e => $"Nom scientifique : {e.NomScientifique}. Id: {e.Id}"));

        [Route("{year:int}/{month:int}")]
        public IActionResult Tags(int year, int month)
        {
            if (month is > 12 or < 1)
            {
                return BadRequest();
            }

            var especes = serviceRecherche.RechercherParMois(year, month).ToList();
            return Content(
                $"{especes.Count} trouvées pour le mois {month} de l'année {year}\n{FormatEspeces(especes)}");
        }
    }
}
