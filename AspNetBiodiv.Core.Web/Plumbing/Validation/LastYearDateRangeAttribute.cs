using System.ComponentModel.DataAnnotations;

namespace AspNetBiodiv.Core.Web.Plumbing.Validation
{
    public class LastYearDateRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var dt = value as DateTime?;
            if (dt == null)
            {
                return new ValidationResult("Not a date.");
            }

            if (dt <= DateTime.Now && dt > DateTime.Now.AddYears(-1))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }
}
