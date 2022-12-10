using AspNetBiodiv.Core.Web.Services.Especes;

namespace AspNetBiodiv.Core.Web.Services.Observations;

public class FakeObservations : IObservations
{
    private readonly ITaxonomie taxonomie;

    public FakeObservations(ITaxonomie taxonomie)
    {
        this.taxonomie = taxonomie;
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

    public IEnumerable<Observation> GetObservationsForEspece(int id)
    {
        return observations.Where(o => o.EspeceObserveeId == id);
    }

    public Observation? GetById(int id)
    {
        var result = observations.SingleOrDefault(o => o.ObservationId == id);
        if (result != null)
        {
            result.EspeceObservee = taxonomie.RechercherParId(result.EspeceObserveeId);
        }
        return result;
    }
}