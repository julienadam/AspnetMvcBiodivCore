using System.ComponentModel.DataAnnotations;

namespace AspNetBiodiv.Core.Web.Models
{
    public class TagSearchViewModel
    {
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Seuls les tags de plus de 2 caractères sont supportés")]
        public string Query { get; set; } = string.Empty;
        public IEnumerable<string>? Results { get; set; }
        public List<string>? PreviousSearches { get; set; } = new List<string>();
    }
}
