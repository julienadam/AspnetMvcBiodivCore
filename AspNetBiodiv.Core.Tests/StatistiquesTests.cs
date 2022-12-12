using AspNetBiodiv.Core.Web.Pages.Statistiques;
using AspNetBiodiv.Core.Web.Services.Statistiques;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;

namespace AspNetBiodiv.Core.Tests;

public class StatistiquesTests
{
    [Fact]
    public void Can_get_stats_if_dates_are_correct()
    {
        var stats = new Mock<IStatisticsService>();
        var model = new IndexModel(stats.Object);
        var dateDebut = new DateTime(2001, 2, 3);
        var dateFin = new DateTime(2001, 3, 4);
        model.Input = new IndexModel.InputModel
        {
            DateDebut = dateDebut,
            DateFin = dateFin
        };

        var statsResult = new UsageStatistics();

        stats
            .Setup(x => x.GetStatsForPeriod(dateDebut, dateFin))
            .Returns(statsResult)
            .Verifiable();

        var result = model.OnPost();

        Assert.NotNull(model.Statistics);
        Assert.NotNull(statsResult);
        Assert.Equal(statsResult, model.Statistics);
        Assert.IsType<PageResult>(result);
        
        stats.VerifyAll();
    }
}