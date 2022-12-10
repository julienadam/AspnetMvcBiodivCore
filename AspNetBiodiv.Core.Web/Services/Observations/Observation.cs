using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspNetBiodiv.Core.Web.Services.Especes;

namespace AspNetBiodiv.Core.Web.Services.Observations;

public class Observation
{
    public int ObservationId { get; set; }
    public DateTime ObservedAt { get; set; }
    public string EmailObservateur { get; set; }
    public int? Individus { get; set; }
    public string NomCommune { get; set; }

    [DataType(DataType.MultilineText)]
    public string Commentaires { get; set; }

    [ForeignKey("EspeceObservee")]
    public int EspeceObserveeId { get; set; }
    public virtual Espece EspeceObservee { get; set; }
}