using Microsoft.AspNetCore.Mvc;

namespace AspNetBiodiv.Core.Web.Controllers
{
    [Route("admin/especes")]
    public class AdminEspecesController : Controller
    {
        [Route("new")]
        public IActionResult Create()
        {
            return Content("Formulaire de saisie d'une nouvelle espèce");
        }

        [Route("new"), HttpPost]
        public IActionResult Create(string data)
        {
            var id = 42;
            return RedirectToAction(nameof(EspecesController.Detail), "Especes", new {id});
        }
    }
}
