using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AspNetBiodiv.Core.Web.Models
{
    public class ObservationViewModel
    {
        public int Id { get; set; }

        [DisplayName("Date d'observation")]
        [DataType(DataType.Date)]
        public DateTime DateObservation { get; set; } = DateTime.Now;

        [DisplayName("Email de l'observateur")]
        [EmailAddress]
        public string EmailObservateur { get; set; }

        [DisplayName("Nom de l'observateur")]
        public string NomObservateur { get; set; }

        [DisplayName("Nombre d'individus observés")]
        public int? Individus { get; set; }

        [DisplayName("Commune du lieu d'observation")]
        public string NomCommune { get; set; }

        [DataType(DataType.MultilineText)]
        public string Commentaires { get; set; }

        public int IdEspeceObservee { get; set; }

        [DisplayName("Nom de l'espèce observée")]
        public string NomEspeceObservee { get; set; }

        public ObservationViewModel(int idEspeceObservee, string nomEspeceObservee)
        {
            IdEspeceObservee = idEspeceObservee;
            NomEspeceObservee = nomEspeceObservee;
        }

        public ObservationViewModel()
        {
        }
    }
}
