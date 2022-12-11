using System.ComponentModel.DataAnnotations;
using AspNetBiodiv.Core.Web.Services.Especes;

namespace AspNetBiodiv.Core.Web.Models
{
    public class TagSearchViewModel
    {
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Seuls les tags de plus de 2 caractères sont supportés")]
        public string Query { get; set; } = string.Empty;
        public IEnumerable<string>? Results { get; set; }
    }
}
