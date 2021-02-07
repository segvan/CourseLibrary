using CourseLibrary.API.Dtos;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.ValidationAttributes
{
    public class CourseTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var course = (CourseCreateDto)validationContext.ObjectInstance;

            if (course.Title == course.Description)
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
