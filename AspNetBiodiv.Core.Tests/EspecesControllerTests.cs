using AspNetBiodiv.Core.Web.Controllers;
using AspNetBiodiv.Core.Web.Models;
using AspNetBiodiv.Core.Web.Services.Especes;
using AspNetBiodiv.Core.Web.Services.Observations;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AspNetBiodiv.Core.Tests;

public class EspecesControllerTests
{
    private readonly Mock<ITaxonomie> taxonomie;
    private readonly Mock<IObservations> observations;
    private readonly EspecesController controller;

    public EspecesControllerTests()
    {
        taxonomie = new Mock<ITaxonomie>();
        observations = new Mock<IObservations>();
        controller = new EspecesController(taxonomie.Object, observations.Object);
    }

    [Fact]
    public void Les_details_sont_retournes_pour_un_id_d_une_espece_existante()
    {
        var espece = new Espece { Id = 42, NomScientifique = "foo bar"};
        taxonomie.Setup(x => x.RechercherParId(42)).Returns(espece);

        var result = controller.Detail(42);

        var view = Assert.IsType<ViewResult>(result);
        var vm = Assert.IsType<EspeceViewModel>(view.Model);
        Assert.Equal(espece.Id, vm.Id);
        Assert.Equal(espece.NomScientifique, vm.NomScientifique);
    }

    [Fact]
    public void Une_erreur_404_est_retournee_pour_un_id_d_une_espece_non_existante()
    {
        taxonomie.Setup(x => x.RechercherParId(42)).Returns((Espece?)null);

        var result = controller.Detail(42);

        Assert.IsType<NotFoundResult>(result);
    }
}