using AspNetBiodiv.Core.Web.Entities;
using Bogus;

namespace AspNetBiodiv.Core.Web.Services.Especes
{
    public class BogusTaxonomie : ITaxonomie
    {
        private readonly Dictionary<int, Espece> especes;

        public BogusTaxonomie()
        {
            var faker = new Faker<Espece>("fr")
                .RuleFor(e => e.EspeceId, f => f.IndexFaker)
                .RuleFor(e => e.NomScientifique, GenerateNomScientifique)
                .RuleFor(e => e.Habitat, f => f.PickRandom<Habitat>())
                .RuleFor(e => e.PresenceEnMetropole, f => f.PickRandom<EtatPresence>())
                .RuleFor(e => e.IdInpn, f => f.UniqueIndex);

            especes = faker.Generate(10).ToDictionary(e => e.EspeceId);
        }

        private static string GenerateNomScientifique(Faker f) =>
            string.Join(" ", f.Lorem.Words(2).Select(w => w.ToLower()));

        public Espece? RechercherParId(int id)
        {
            especes.TryGetValue(id, out var found);
            return found;
        }

        public Espece? RechercherParNomScientifique(string nomScientifique)
        {
            return especes.Values.FirstOrDefault(e => 
                string.Compare(e.NomScientifique, nomScientifique, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public IEnumerable<Espece> RechercherParTag(string tag) =>
            tag =="foo" ? Enumerable.Empty<Espece>() : especes.Values;

        public IEnumerable<Espece> RechercherParMois(int year, int month) =>
            year == 2022 && month == 2 ? Enumerable.Empty<Espece>() : especes.Values;

        private IEnumerable<string> AllTags()
        {
            yield return "vulpes";
            yield return "mammifère";
            yield return "canidés";
            yield return "canis";
            yield return "poisson";
        }

        public IEnumerable<string> RechercheDeTags(string query)
        {
            return AllTags().Where(t => t.Contains(query.ToLower()));
        }
    }
}
