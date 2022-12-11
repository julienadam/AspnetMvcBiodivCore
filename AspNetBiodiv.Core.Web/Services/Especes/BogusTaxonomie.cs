﻿using Bogus;

namespace AspNetBiodiv.Core.Web.Services.Especes
{
    public class BogusTaxonomie : ITaxonomie
    {
        private readonly Faker<Espece> faker;

        public BogusTaxonomie()
        {
            faker = new Faker<Espece>("fr")
                .RuleFor(e => e.Habitat, f => f.PickRandom<Habitat>())
                .RuleFor(e => e.Presence, f => f.PickRandom<EtatPresence>())
                .RuleFor(e => e.IdInpn, f => f.UniqueIndex);
        }

        private static string GenerateNomScientifique(Faker f) =>
            string.Join(" ", f.Lorem.Words(2).Select(w => w.ToLower()));

        private IEnumerable<Espece> GenerateSomeEspeces()
        {
            return faker
                .RuleFor(e => e.Id, f => f.IndexFaker)
                .RuleFor(e => e.NomScientifique, GenerateNomScientifique)

                .GenerateBetween(1, 10);
        }

        public Espece? RechercherParId(int id)
        {
            if (id == 42)
            {
                return null;
            }

            return faker
                .RuleFor(e => e.Id, id)
                .RuleFor(e => e.NomScientifique, GenerateNomScientifique)
                .Generate();
        }

        public Espece? RechercherParNomScientifique(string nomScientifique)
        {
            if (nomScientifique == "foo")
            {
                return null;
            }

            return faker
                .RuleFor(e => e.Id, f => f.IndexFaker)
                .RuleFor(e => e.NomScientifique, nomScientifique)
                .Generate();
        }

        public IEnumerable<Espece> RechercherParTag(string tag) =>
            tag == "foo" ? new List<Espece>() : GenerateSomeEspeces();

        public IEnumerable<Espece> RechercherParMois(int year, int month) =>
            year == 2022 && month == 2 ? new List<Espece>() : GenerateSomeEspeces();
    }
}