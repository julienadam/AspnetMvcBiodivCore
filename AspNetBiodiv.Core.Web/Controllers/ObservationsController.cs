using AspNetBiodiv.Core.Web.Entities;
using AspNetBiodiv.Core.Web.Models;
using AspNetBiodiv.Core.Web.Services.Especes;
using AspNetBiodiv.Core.Web.Services.Observations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetBiodiv.Core.Web.Controllers
{
    [Route("observations")]
    [Authorize]
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
            var viewModel = new ObservationViewModel(espece.EspeceId, espece.NomScientifique)
            {
                DateCreation = DateTime.Now,
                // NomCommune = User.Claims.FirstOrDefault(c => c.Type == "commune")?.Value ?? ""
            };
            return View(viewModel);
        }

        [Route("{id_espece:int}/saisie")]
        [HttpPost]
        public ActionResult Create(int id_espece, ObservationViewModel viewModel)
        {
            if (HasTooManyToday(GetEmailFromUser()))
            {
                ModelState.AddModelError("Email", "Too many posts today");
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            
            var espece = taxonomie.RechercherParId(id_espece);
            observations.Create(CreateObservationFromViewModel(id_espece, viewModel, espece));
            TempData["ThankYou"] = "Merci pour votre saisie !";
            return RedirectToAction("Detail", "Especes", new { id = id_espece });
        }

        private string GetEmailFromUser() => User?.Identity?.Name ?? "";

        private Observation CreateObservationFromViewModel(int id_espece, ObservationViewModel viewModel, Espece? espece)
        {
            return new Observation
            {
                PostedAt = viewModel.DateCreation ?? DateTime.Now,
                Commentaires = viewModel.Commentaires ?? "",
                EspeceObserveeEspeceId = id_espece,
                EmailObservateur = GetEmailFromUser(),
                NomCommune = viewModel.NomCommune,
                Individus = viewModel.Individus,
                ObservedAt = viewModel.DateObservation,
                EspeceObservee = espece
            };
        }

        [Route("{id:int}/confirm-delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var observation = observations.GetById(id);
            if (observation == null)
            {
                return NotFound();
            }

            if (!CanCurrentUserEditThisObservation(observation))
            {
                return Unauthorized();
            }

            ViewBag.Id = id;
            return View();
        }

        [Route("{id:int}/delete")]
        public ActionResult Delete(int id)
        {
            var observation = observations.GetById(id);
            if (observation == null)
            {
                return NotFound();
            }

            if (!CanCurrentUserEditThisObservation(observation))
            {
                return Unauthorized();
            }

            observations.Delete(observation);
            var especeId = observation.EspeceObserveeEspeceId;
            return RedirectToAction("Detail", "Especes", new { id = especeId });

        }


        [Route("{id:int}/edit")]
        public ActionResult Edit(int id)
        {
            var observation = observations.GetById(id);
            if (observation == null)
            {
                return NotFound();
            }

            if (!CanCurrentUserEditThisObservation(observation))
            {
                return Unauthorized();
            }

            return View(ObservationViewModel.FromObservation(observation));

        }

        private bool CanCurrentUserEditThisObservation(Observation observation) => 
            string.Equals(observation.EmailObservateur, GetEmailFromUser(), StringComparison.InvariantCultureIgnoreCase);

        [Route("{id:int}/edit")]
        [HttpPost]
        public ActionResult Edit(ObservationViewModel input, int id)
        {
            var existing = observations.GetById(id);
            if (existing == null)
            {
                return NotFound();
            }

            if (!CanCurrentUserEditThisObservation(existing))
            {
                return Unauthorized();
            }
            
            // Pas besoin de valider le nombre de posts
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            UpdateObservationFromViewModel(existing, input);
            observations.Update(existing);
            return RedirectToAction("Detail", "Especes", new { id = existing.EspeceObserveeEspeceId });
        }

        private static void UpdateObservationFromViewModel(Observation observation, ObservationViewModel input)
        {
            observation.Commentaires = input.Commentaires ?? "";
            observation.NomCommune = input.NomCommune ?? "";
            observation.Individus = input.Individus;
            observation.ObservedAt = input.DateObservation;
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

        private static ObservationViewModel CreateViewModel(Observation observation)
        {
            return new ObservationViewModel
            {
                DateCreation = observation.PostedAt,
                NomEspeceObservee = observation.EspeceObservee.NomScientifique,
                NomCommune = observation.NomCommune,
                Commentaires = observation.Commentaires,
                Individus = observation.Individus,
                DateObservation = observation.ObservedAt,
                Id = observation.ObservationId,
                IdEspeceObservee = observation.EspeceObserveeEspeceId
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
