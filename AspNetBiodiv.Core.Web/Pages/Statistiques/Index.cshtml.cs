using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AspNetBiodiv.Core.Web.Services.Statistiques;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetBiodiv.Core.Web.Pages.Statistiques
{
    public class IndexModel : PageModel
    {
        private readonly IStatisticsService statisticsService;

        public IndexModel(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        public class InputModel : IValidatableObject
        {
            [DisplayName("Date de début")]
            [DataType(DataType.Date)]
            [Required]
            public DateTime? DateDebut { get; set; } = DateTime.Now.AddMonths(-1).Date;

            [DisplayName("Date de fin")]
            [DataType(DataType.Date)]
            [Required]
            public DateTime? DateFin { get; set; } = DateTime.Now.Date;

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (DateFin <= DateDebut)
                {
                    yield return new ValidationResult("Date de fin doit être après la date de début",
                        new[] { nameof(DateDebut) });
                }
            }
        }

        public void OnGet()
        {
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Input.DateDebut is null || Input.DateFin is null)
            {
                return RedirectToPage();
            }

            var stats = statisticsService.GetStatsForPeriod(Input.DateDebut.Value, Input.DateFin.Value);
            Statistics = stats;

            return Page();
        }

       
        public UsageStatistics? Statistics { get; set; }

    }
}