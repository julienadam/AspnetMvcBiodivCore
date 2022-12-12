using Microsoft.AspNetCore.Mvc;

namespace AspNetBiodiv.Core.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index(int id)
        {
            if (id == 404)
            {
                return Redirect("/404.html");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
