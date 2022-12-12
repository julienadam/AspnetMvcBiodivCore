using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AspNetBiodiv.Core.Web.Entities
{
    [Index(nameof(NomScientifique), IsUnique = true)]
    public class Espece
    {
        public int EspeceId { get; set; }

        [Required]
        public string NomScientifique { get; set; }
        public EtatPresence PresenceEnMetropole { get; set; }
        public Habitat Habitat { get; set; }
        public int IdInpn { get; set; }

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}