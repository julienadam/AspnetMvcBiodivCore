using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetBiodiv.Core.Web.Entities
{
    public class Observation
    {
        public int ObservationId { get; set; }
        [Required]
        public DateTime PostedAt { get; set; }
        [Required]
        public DateTime ObservedAt { get; set; }
        [Required]
        public string EmailObservateur { get; set; }
        public int? Individus { get; set; }
        [Required]
        public string NomCommune { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Commentaires { get; set; }

        [ForeignKey(nameof(EspeceObservee))]
        public int EspeceObserveeEspeceId { get; set; }

        public virtual Espece EspeceObservee { get; set; }
    }
}
