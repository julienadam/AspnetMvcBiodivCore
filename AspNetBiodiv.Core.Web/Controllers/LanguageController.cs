using Microsoft.AspNetCore.Mvc;

namespace AspNetBiodiv.Core.Web.Controllers;

public class LanguageController : Controller
{
    // GET
    public IActionResult SetLang(string lang)
    {
        Response.Cookies.Append("lang", lang);
        return RedirectToAction("Index", "Home");
    }
}