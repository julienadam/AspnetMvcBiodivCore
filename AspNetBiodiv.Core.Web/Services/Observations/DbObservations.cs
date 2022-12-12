using AspNetBiodiv.Core.Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetBiodiv.Core.Web.Services.Observations;

public class DbObservations : IObservations
{
    private readonly EspecesContext context;

    public DbObservations(EspecesContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public int Create(Observation observation)
    {
        context.Add(observation);
        context.SaveChanges();
        return observation.ObservationId;
    }

    public IEnumerable<Observation>? GetObservationsForEspece(int id)
    {
        return context.Observations
            .Include(o => o.EspeceObservee)
            .Where(o => o.EspeceObserveeId == id)
            .ToList();
    }

    public Observation? GetById(int id)
    {
        return context.Observations
            .Include(o => o.EspeceObservee)
            .SingleOrDefault(o => o.ObservationId == id);
    }

    public void Delete(Observation observation)
    {
        context.Observations.Remove(observation);
        context.SaveChanges();
    }

    public void Update(Observation observation)
    {
        context.Observations.Attach(observation);
        context.SaveChanges();
    }

    public int NumberOfObservationsToday(string email)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        return context.Observations.Count(
            o =>
                string.Equals(o.EmailObservateur, email, StringComparison.CurrentCultureIgnoreCase)
                && o.PostedAt >= today && o.PostedAt <= tomorrow);
    }

    private readonly Random random = new Random();

    public Observation? GetRandom()
    {
        var last = context.Observations
            .OrderByDescending(o => o.ObservationId)
            .Select(o => o.ObservationId)
            .FirstOrDefault();

        var rndId = random.Next(1, last);
        return context.Observations.FirstOrDefault(o => o.ObservationId == rndId);
    }
}