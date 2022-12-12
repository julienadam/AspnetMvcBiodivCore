using AspNetBiodiv.Core.Web.Services.Especes;
using AspNetBiodiv.Core.Web.Services.Observations;
using Microsoft.AspNetCore.Mvc;

namespace AspNetBiodiv.Core.Web.Components
{
    public class LanguageSelectViewComponent : ViewComponent
    {
        public LanguageSelectViewComponent()
        {
        }

        public IViewComponentResult Invoke()
        {
            var lang = Request.Cookies["lang"] ?? "fr";
            return View("_LanguageSelect", lang);
        }
    }
}
