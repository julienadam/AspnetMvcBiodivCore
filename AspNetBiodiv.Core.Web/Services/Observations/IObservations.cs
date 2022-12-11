namespace AspNetBiodiv.Core.Web.Services.Observations
{
    public interface IObservations
    {
        int Create(Observation observation);
        IEnumerable<Observation>? GetObservationsForEspece(int id);
        Observation? GetById(int id);
        void Delete(Observation observation);
        void Update(Observation observation);
    }
}
