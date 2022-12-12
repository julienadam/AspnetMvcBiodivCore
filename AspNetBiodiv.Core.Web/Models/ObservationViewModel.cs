using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using AspNetBiodiv.Core.Web.Entities;
using AspNetBiodiv.Core.Web.Services.Observations;
using AspNetBiodiv.Core.Web.Plumbing.Validation;
using AspNetBiodiv.Core.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetBiodiv.Core.Web.Models
{
    public class ObservationViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [DisplayName("Date d'observation")]
        [Required(ErrorMessage = "Selectionner une date valide")]
        [DataType(DataType.Date)]
        [LastYearDateRange(ErrorMessage = "Choisissez une date dans l'année passée")]
        public DateTime DateObservation { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Saisissez une adresse email valide")]
        [DisplayName("Email de l'observateur")]
        [Remote(action: "ValidateNumberOfPosts", controller: "Observations")]
        public string EmailObservateur { get; set; } = string.Empty;

        [DisplayName("Nombre d'individus observés")]
        [Range(1, 100, ErrorMessage = "Saisissez un nombre entre 1 et 100")]
        public int? Individus { get; set; }

        [DisplayName("Commune du lieu d'observation")]
        [Required(ErrorMessage = "Choisissez une ville dans la liste")]
        public string NomCommune { get; set; } = string.Empty;

        [DataType(DataType.MultilineText)] public string? Commentaires { get; set; }

        public int IdEspeceObservee { get; set; }

        [DisplayName("Nom de l'espèce observée")]
        public string? NomEspeceObservee { get; set; }

        public DateTime? DateCreation { get; set; }

        public ObservationViewModel(int idEspeceObservee, string nomEspeceObservee)
        {
            IdEspeceObservee = idEspeceObservee;
            NomEspeceObservee = nomEspeceObservee;
        }

        public ObservationViewModel()
        {
        }

        public static ObservationViewModel FromObservation(Observation observation)
        {
            return new ObservationViewModel
            {
                Id = observation.ObservationId,
                NomEspeceObservee = observation.EspeceObservee.NomScientifique,
                Commentaires = observation.Commentaires,
                DateObservation = observation.ObservedAt,
                EmailObservateur = observation.EmailObservateur,
                Individus = observation.Individus,
                NomCommune = observation.NomCommune,
                IdEspeceObservee = observation.EspeceObserveeEspeceId
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (new StaticCommunes().GetCommunes().Any(c => c == NomCommune))
            {
                yield break;
            }

            yield return new ValidationResult(
                $"Aucune commune nommée {NomCommune}", 
                new[] { nameof(NomCommune) });
        }
    }
}