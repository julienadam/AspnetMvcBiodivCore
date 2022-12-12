using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetBiodiv.Core.Web.Services;

public interface ICommunes
{
    IEnumerable<string> GetCommunes();
    IEnumerable<SelectListItem> GetCommuneItems();
}