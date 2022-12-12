using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AspNetBiodiv.Core.Web.Services.Statistiques;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetBiodiv.Core.Web.Pages.Statistiques
{
    public class IndexModel : PageModel, IValidatableObject
    {
        private readonly IStatisticsService statisticsService;

        public IndexModel(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (DateDebut is null || DateFin is null)
            {
                return RedirectToPage();
            }

            var stats = statisticsService.GetStatsForPeriod(DateDebut.Value, DateFin.Value);
            Statistics = stats;

            return Page();
        }

        [DisplayName("Date de début")]
        [DataType(DataType.Date)]
        [Required]
        [BindProperty]
        public DateTime? DateDebut { get; set; } = DateTime.Now.AddMonths(-1).Date;

        [DisplayName("Date de fin")]
        [DataType(DataType.Date)]
        [Required]
        [BindProperty]
        public DateTime? DateFin { get; set; } = DateTime.Now.Date;

        public UsageStatistics? Statistics { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateFin <= DateDebut)
            {
                yield return new ValidationResult("Date de fin doit être après la date de début",
                    new[] { nameof(DateDebut), nameof(DateFin) });
            }
        }
    }
}