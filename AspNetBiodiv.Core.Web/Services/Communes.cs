using Microsoft.AspNetCore.Mvc.Rendering;

static internal class Communes
{
    public static IEnumerable<SelectListItem> GetCommunes()
    {
        yield return new SelectListItem { Value = "Aradon", Text = "Aradon" };
        yield return new SelectListItem { Value = "Arz", Text = "Arz" };
        yield return new SelectListItem { Value = "Elven", Text = "Elven" };
        yield return new SelectListItem { Value = "Theix-Noyalo", Text = "Theix-Noyalo" };
        yield return new SelectListItem { Value = "Trédion", Text = "Trédion" };
        yield return new SelectListItem { Value = "Vannes", Text = "Vannes" };
    }
}