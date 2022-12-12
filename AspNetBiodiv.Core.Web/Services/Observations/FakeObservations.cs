using AspNetBiodiv.Core.Web.Entities;
using AspNetBiodiv.Core.Web.Services.Especes;
using Bogus;

namespace AspNetBiodiv.Core.Web.Services.Observations;

public class FakeObservations : IObservations
{
    private readonly ITaxonomie taxonomie;

    public FakeObservations(ITaxonomie taxonomie, ICommunes communes)
    {
        this.taxonomie = taxonomie;
        var espece = taxonomie.RechercherParId(0);

        var faker = new Faker<Observation>()
            .RuleFor(o => o.Commentaires, f => f.Lorem.Paragraphs(2))
            .RuleFor(o => o.NomCommune, f => f.PickRandom(communes.GetCommunes()))
            .RuleFor(o => o.ObservedAt, f => f.Date.Past())
            .RuleFor(o => o.EmailObservateur, f => f.Person.Email)
            .RuleFor(o => o.Individus, f => f.Random.Int(1, 4))
            .RuleFor(o => o.EspeceObserveeEspeceId, f => 0)
            .RuleFor(o => o.EspeceObservee, f => espece)
            .RuleFor(o => o.ObservationId, f => f.IndexFaker);
        
        observations.AddRange(faker.Generate(7));
    }


    private int currentId = 876;
    private readonly List<Observation> observations = new();

    public int Create(Observation observation)
    {
        var result = Interlocked.Increment(ref currentId);
        observation.ObservationId = result;
        observations.Add(observation);
        return currentId;
    }

    public IEnumerable<Observation>? GetObservationsForEspece(int id)
    {
        return observations.Where(o => o.EspeceObserveeEspeceId == id);
    }

    public Observation? GetById(int id)
    {
        var result = observations.SingleOrDefault(o => o.ObservationId == id);
        if (result == null)
        {
            return result;
        }
        
        var e = taxonomie.RechercherParId(result.EspeceObserveeEspeceId);
        if (e == null)
        {
            throw new KeyNotFoundException($"Aucune espèce avec l'id {id}");
        }

        result.EspeceObservee = e;
        return result;
    }

    public void Delete(Observation observation)
    {
        observations.Remove(observation);
    }

    public void Update(Observation observation)
    {
        var e = GetById(observation.ObservationId);
        if (e == null)
        {
            return;
        }

        Delete(e);
        observations.Add(observation);
    }

    public int NumberOfObservationsToday(string email)
    {
        return observations.Count(o =>
            o.EmailObservateur == email && o.PostedAt.Date == DateTime.Now.Date);
    }

    private static readonly Random Rnd = new();

    public Observation? GetRandom() => 
        observations[Rnd.Next(0, observations.Count)];
}