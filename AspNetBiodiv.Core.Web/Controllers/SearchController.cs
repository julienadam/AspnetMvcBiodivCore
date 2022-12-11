using AspNetBiodiv.Core.Web.Models;
using AspNetBiodiv.Core.Web.Services.Especes;
using Microsoft.AspNetCore.Mvc;

namespace AspNetBiodiv.Core.Web.Controllers
{
    [Route("search")]
    public class SearchController : Controller
    {
        private readonly ITaxonomie taxonomie;

        public SearchController(ITaxonomie taxonomie)
        {
            this.taxonomie = taxonomie;
        }

        [Route("")]
        public ActionResult Index()
        {
            var input = new TagSearchViewModel();
            return View(input);
        }

        [Route("")]
        [HttpPost]
        public ActionResult Index(TagSearchViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }
            
            var tags =
                taxonomie.RechercheDeTags(input.Query)
                    .OrderBy(i => i);

            return View(new TagSearchViewModel
            {
                Query = input.Query,
                Results = tags,
            });
        }
    }
}
