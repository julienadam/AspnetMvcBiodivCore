using AspNetBiodiv.Core.Web.Models;
using AspNetBiodiv.Core.Web.Services.Especes;
using AspNetBiodiv.Core.Web.Services.Observations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetBiodiv.Core.Web.Controllers
{
    [Route("observations")]
    public class ObservationsController : Controller
    {
        private readonly ITaxonomie taxonomie;
        private readonly IObservations observations;

        public ObservationsController(ITaxonomie taxonomie, IObservations observations)
        {
            this.taxonomie = taxonomie;
            this.observations = observations;
        }

        [Route("{id_espece:int}/saisie")]
        public ActionResult Create(int id_espece)
        {
            ViewBag.Communes = GetCommunes();
            var espece = taxonomie.RechercherParId(id_espece);
            var viewModel = new ObservationViewModel(espece.Id, espece.NomScientifique);
            return View(viewModel);
        }

        private static IEnumerable<SelectListItem> GetCommunes()
        {
            yield return new SelectListItem { Value = "Aradon", Text = "Aradon" };
            yield return new SelectListItem { Value = "Arz", Text = "Arz" };
            yield return new SelectListItem { Value = "Elven", Text = "Elven" };
            yield return new SelectListItem { Value = "Theix-Noyalo", Text = "Theix-Noyalo" };
            yield return new SelectListItem { Value = "Trédion", Text = "Trédion" };
            yield return new SelectListItem { Value = "Vannes", Text = "Vannes" };
        }

        [Route("{id_espece:int}/saisie")]
        [HttpPost]
        public ActionResult Create(int id_espece, ObservationViewModel viewModel)
        {
            var espece = taxonomie.RechercherParId(id_espece);
            observations.Create(new Observation
            {
                Commentaires = viewModel.Commentaires,
                EspeceObserveeId = id_espece,
                EmailObservateur = viewModel.EmailObservateur,
                NomCommune = viewModel.NomCommune,
                Individus = viewModel.Individus,
                ObservedAt = viewModel.DateObservation,
                EspeceObservee = espece
            });

            return RedirectToAction("Detail", "Especes", new { id = id_espece });
        }

        public ActionResult Details(int id)
        {
            var obs = observations.GetById(id);

            if (obs == null)
            {
                return NotFound();
            }
            return View(CreateViewModel(obs));
        }

        private ObservationViewModel CreateViewModel(Observation observation)
        {
            return new ObservationViewModel
            {
                NomEspeceObservee = observation.EspeceObservee?.NomScientifique,
                NomCommune = observation.NomCommune,
                Commentaires = observation.Commentaires,
                Individus = observation.Individus,
                DateObservation = observation.ObservedAt,
                EmailObservateur = observation.EmailObservateur,
                Id = observation.ObservationId,
                IdEspeceObservee = observation.EspeceObserveeId
            };
        }

        //// GET: Observations
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: Observations/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}




        //// GET: Observations/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Observations/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Observations/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Observations/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
