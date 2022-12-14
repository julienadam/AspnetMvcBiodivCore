using AspNetBiodiv.Core.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetBiodiv.Core.Web.Controllers
{
    [Route("admin/especes")]
    public class AdminEspecesController : Controller
    {
        private readonly IImportService importService;

        public AdminEspecesController(IImportService importService)
        {
            this.importService = importService;
        }

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


        [Route("import")]
        public IActionResult Import()
        {
            importService.Import();

            return Content("Import done");
        }


    }
}
