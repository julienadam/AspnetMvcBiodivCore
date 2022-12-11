using AspNetBiodiv.Core.Web.Models;
using AspNetBiodiv.Core.Web.Services.Especes;
using AspNetBiodiv.Core.Web.Services.Observations;
using Microsoft.AspNetCore.Mvc;

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
            var espece = taxonomie.RechercherParId(id_espece);
            var viewModel = new ObservationViewModel(espece.Id, espece.NomScientifique)
            {
                DateCreation = DateTime.Now
            };
            return View(viewModel);
        }

        [Route("{id_espece:int}/saisie")]
        [HttpPost]
        public ActionResult Create(int id_espece, ObservationViewModel viewModel)
        {
            if (HasTooManyToday(viewModel.EmailObservateur))
            {
                ModelState.AddModelError(nameof(ObservationViewModel.EmailObservateur), "Too many posts today");
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            
            var espece = taxonomie.RechercherParId(id_espece);
            observations.Create(CreateObservationFromViewModel(id_espece, viewModel, espece));
            return RedirectToAction("Detail", "Especes", new { id = id_espece });
        }

        private static Observation CreateObservationFromViewModel(int id_espece, ObservationViewModel viewModel, Espece? espece)
        {
            return new Observation
            {
                PostedAt = viewModel.DateCreation ?? DateTime.Now,
                Commentaires = viewModel.Commentaires ?? "",
                EspeceObserveeId = id_espece,
                EmailObservateur = viewModel.EmailObservateur,
                NomCommune = viewModel.NomCommune,
                Individus = viewModel.Individus,
                ObservedAt = viewModel.DateObservation,
                EspeceObservee = espece
            };
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


        [Route("{id:int}/edit")]
        public ActionResult Edit(int id)
        {
            var observation = observations.GetById(id);
            if (observation != null)
            {
                return View(ObservationViewModel.FromObservation(observation));
            }
            else
            {
                return NotFound();
            }
        }

        [Route("{id:int}/edit")]
        [HttpPost]
        public ActionResult Edit(ObservationViewModel input, int id)
        {
            var existing = observations.GetById(id);
            if (existing == null)
            {
                return NotFound();
            }

            if (existing.EmailObservateur != input.EmailObservateur)
            {
                ModelState.AddModelError(nameof(ObservationViewModel.EmailObservateur), "L'email a été changé !!!");
            }
            
            // Pas besoin de valider le nombre de posts
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var observation = CreateObservationFromViewModel(existing.EspeceObserveeId, input, existing.EspeceObservee);
            observation.ObservationId = id;
            observations.Update(observation);
            return RedirectToAction("Detail", "Especes", new { id = observation.EspeceObserveeId });
        }

        [Route("{id:int}")]
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
                DateCreation = observation.PostedAt,
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

        [AcceptVerbs("GET", "POST")]
        public IActionResult ValidateNumberOfPosts(string emailObservateur) =>
            HasTooManyToday(emailObservateur) 
                ? Json("Vous avez déja envoyé 5 observations aujourd'hui !") 
                : Json(true);

        private bool HasTooManyToday(string emailObservateur) 
            => observations.NumberOfObservationsToday(emailObservateur) >= 5;
    }
}
