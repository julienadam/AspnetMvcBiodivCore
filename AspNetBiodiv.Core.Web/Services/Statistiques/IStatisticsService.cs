namespace AspNetBiodiv.Core.Web.Services.Statistiques;

public interface IStatisticsService
{
    UsageStatistics GetStatsForPeriod(DateTime dateDebut, DateTime dateFin);
}