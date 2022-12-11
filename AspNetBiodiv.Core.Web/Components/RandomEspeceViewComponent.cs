using AspNetBiodiv.Core.Web.Services.Especes;
using AspNetBiodiv.Core.Web.Services.Observations;
using Microsoft.AspNetCore.Mvc;

namespace AspNetBiodiv.Core.Web.Components
{
    public class RandomEspeceViewModel
    {
        public string NomScientifique { get; set; } = "";
        public int Id { get; set; }
        public int CodeInpn { get; set; }
    }

    public class RandomEspeceViewComponent : ViewComponent
    {
        private readonly IObservations observations;

        public RandomEspeceViewComponent(IObservations observations)
        {
            this.observations = observations;
        }

        public IViewComponentResult Invoke()
        {
            var o = observations.GetRandom();
            var espece = o.EspeceObservee;
            var vm = new RandomEspeceViewModel
            {
                Id = espece.Id,
                CodeInpn = espece.IdInpn,
                NomScientifique = espece.NomScientifique
            };
            return View("_Random", vm);
        }
    }
}
