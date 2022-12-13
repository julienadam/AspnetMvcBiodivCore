using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetBiodiv.Core.Web.Services;

public class StaticCommunes : ICommunes
{
    public IEnumerable<SelectListItem> GetCommuneItems()
    {
        return GetCommunes().Select(c => new SelectListItem { Text = c, Value = c }).ToList();
    }

    public IEnumerable<string> GetCommunes()
    {
        yield return "Aradon";
        yield return "Arz";
        yield return "Elven";
        yield return "Theix-Noyalo";
        yield return "Trédion";
        yield return "Vannes";
    }
}