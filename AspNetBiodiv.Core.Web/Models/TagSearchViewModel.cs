using System.ComponentModel.DataAnnotations;
using AspNetBiodiv.Core.Web.Services.Especes;

namespace AspNetBiodiv.Core.Web.Models
{
    public class TagSearchViewModel
    {
        public string Query { get; set; }
        public IEnumerable<string> Results { get; set; }
    }
}
