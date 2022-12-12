using System;
using System.ComponentModel.DataAnnotations;
using AspNetBiodiv.Core.Web.Plumbing.Validation;

namespace AspNetBiodiv.Core.Tests;

public class LastYearDateRangeAttributeTests
{
    private readonly LastYearDateRangeAttribute attribute = new()
    {
        ErrorMessage = "TheError"
    };

    [Fact]
    public void Two_years_ago_is_not_valid()
    {
        var tooFarBack = DateTime.Now.AddYears(-2);

        var result = attribute.GetValidationResult(tooFarBack, new ValidationContext(new object()));

        Assert.NotNull(result);
        Assert.Equal(attribute.ErrorMessage, result.ErrorMessage);
    }

    [Fact]
    public void Next_month_is_invalid()
    {
        var inTheFuture = DateTime.Now.AddMonths(1);

        var result = attribute.GetValidationResult(inTheFuture, new ValidationContext(new object()));

        Assert.NotNull(result);
        Assert.Equal(attribute.ErrorMessage, result.ErrorMessage);
    }


    [Fact]
    public void Last_month_is_valid()
    {
        var lastMonth = DateTime.Now.AddMonths(-1);

        var result = attribute.GetValidationResult(lastMonth, new ValidationContext(new object()));

        Assert.Equal(ValidationResult.Success, result);
    }
}