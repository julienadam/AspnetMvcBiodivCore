using System.Text.Json;
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
            var input = new TagSearchViewModel
            {
                PreviousSearches = GetSearchesFromSession()
            };

            return View(input);
        }

        private List<string> GetSearchesFromSession()
        {
            var queries = HttpContext.Session.GetString("TagSearches") ?? "[]";
            return JsonSerializer.Deserialize<List<string>>(queries) ?? new List<string>();
        }

        private List<string> AppendSearchToSession(string query)
        {
            var searches = GetSearchesFromSession();
            searches.Add(query);
            // TODO: remember only the last 10 ones
            var serializedSearches = JsonSerializer.Serialize(searches);
            HttpContext.Session.SetString("TagSearches", serializedSearches);
            return searches;
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
                PreviousSearches = AppendSearchToSession(input.Query)
            });
        }
    }
}
