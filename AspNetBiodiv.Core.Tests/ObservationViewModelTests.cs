using System.ComponentModel.DataAnnotations;
using AspNetBiodiv.Core.Web.Models;
using AspNetBiodiv.Core.Web.Services;

namespace AspNetBiodiv.Core.Tests;

public class ObservationViewModelTests
{
    [Fact]
    public void Foo_is_not_a_valid_commune()
    {
        var vm = new ObservationViewModel
        {
            NomCommune = "Foo"
        };


        var result = vm.Validate(new ValidationContext(vm));
        Assert.NotNull(result);
        Assert.Contains(result, 
            x => 
                x.MemberNames.Contains(nameof(ObservationViewModel.NomCommune)));
    }

    [Fact]
    public void First_static_commune_is_valid()
    {
        var vm = new ObservationViewModel
        {
            NomCommune = new StaticCommunes().GetCommunes().First()
        };


        var result = vm.Validate(new ValidationContext(vm));
        Assert.Empty(result);
    }
}