using AspNetBiodiv.Core.Web.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AspNetBiodiv.Core.Web.Services.Observations;

public class DbObservations : IObservations
{
    private readonly EspecesContext context;
    private readonly ILogger<DbObservations> logger;

    public DbObservations(EspecesContext context, ILogger<DbObservations> logger)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.logger = logger;
    }

    public int Create(Observation observation)
    {
        logger.LogInformation("New observation for {espece}", observation.EspeceObservee.NomScientifique);
        context.Add(observation);
        context.SaveChanges();
        return observation.ObservationId;
    }

    public IEnumerable<Observation>? GetObservationsForEspece(int id)
    {
        return context.Observations
            .Include(o => o.EspeceObservee)
            .Where(o => o.EspeceObserveeEspeceId == id)
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
        context.Entry(observation).State = EntityState.Modified;
        // context.Observations.Attach(observation);
        context.SaveChanges();
    }

    public int NumberOfObservationsToday(string email)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        return context.Observations.Count(
            o =>
                (o.EmailObservateur.ToLower() == email.ToLower())
                && o.PostedAt >= today && o.PostedAt <= tomorrow);
    }

    private readonly Random random = new Random();

    public Observation? GetRandom()
    {
        var last = context.Observations
            .OrderByDescending(o => o.ObservationId)
            .Select(o => o.ObservationId)
            .FirstOrDefault();

        if (last == 0)
        {
            logger.LogWarning("No random observations could be determined !");
            return null;
        }

        var rndId = random.Next(1, last);
        var result = context.Observations
            .Include(o => o.EspeceObservee)
            .FirstOrDefault(o => o.ObservationId == rndId);

        if (result == null)
        {
            logger.LogError("No observation with id : {id}", rndId);
        }

        return result;
    }
}