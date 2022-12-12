using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AspNetBiodiv.Core.Web.Entities
{
  [Index(nameof(Nom), IsUnique = true)]
  public class Tag {
    public int TagId { get; set; }

    [Required]
    public string Nom { get; set; }

    public ICollection<Espece> Especes { get; set; } = new List<Espece>();
  }
}