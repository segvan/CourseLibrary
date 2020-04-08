using CourseLibrary.API.Dtos;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.ValidationAttributes
{
    public class CourceTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var cource = (CourseCreateDto)validationContext.ObjectInstance;

            if (cource.Title == cource.Description)
            {
                return new ValidationResult(
                    ErrorMessage ?? "Description should be different from Title",
                    new[] { nameof(CourseCreateDto) }
                );
            }

            return ValidationResult.Success;
        }
    }
}
