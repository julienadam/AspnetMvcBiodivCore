namespace AspNetBiodiv.Core.Web.Services.Statistiques;

public class UsageStatistics
{
    public int NbObservations { get; init; }

    public int ObservateursUniques { get; init; }

    public int EspecesUniques { get; init; }

    public IReadOnlyDictionary<string, int> ObservationsParVille { get; init; }
    /*
         * 
    le nombres d'observations
    le nombre d'observateurs uniques
    le nombre d'espèces uniques observées
    le nombre d'observations par ville

         */
}