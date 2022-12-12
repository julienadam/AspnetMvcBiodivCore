namespace AspNetBiodiv.Core.Web.Services.Statistiques
{
    public class StatisticsService : IStatisticsService
    {
        public UsageStatistics GetStatsForPeriod(DateTime dateDebut, DateTime dateFin) =>
            new()
            {
                EspecesUniques = 42,
                NbObservations = 129,
                ObservateursUniques = 76,
                ObservationsParVille = new Dictionary<string, int>
                {
                    { "Vannes", 65 },
                    { "Trédion", 19 }
                }
            };
    }
}
