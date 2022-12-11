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
            ViewBag.Communes = Communes.GetCommunes();
            var espece = taxonomie.RechercherParId(id_espece);
            var viewModel = new ObservationViewModel(espece.Id, espece.NomScientifique);
            return View(viewModel);
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

        [Route("{id:int}/confirm-delete")]
        public ActionResult DeleteConfirm(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        [Route("{id:int}/delete")]
        public ActionResult Delete(int id)
        {
            var observation = observations.GetById(id);
            if (observation != null)
            {
                observations.Delete(observation);
                var especeId = observation.EspeceObserveeId;
                return RedirectToAction("Detail", "Especes", new { id = especeId });
            }
            else
            {
                return NotFound();
            }
        }

        [Route("")]
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
                NomEspeceObservee = observation.EspeceObservee.NomScientifique,
                NomCommune = observation.NomCommune,
                Commentaires = observation.Commentaires,
                Individus = observation.Individus,
                DateObservation = observation.ObservedAt,
                EmailObservateur = observation.EmailObservateur,
                Id = observation.ObservationId,
                IdEspeceObservee = observation.EspeceObserveeId
            };
        }

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
    }
}
