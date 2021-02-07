using CourseLibrary.API.ValidationAttributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Dtos
{
    [CourseTitleMustBeDifferentFromDescriptionAttribute]
    public class CourseCreateDto //: IValidatableObject
    {
        [Required(ErrorMessage ="Title should be not empty")]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1500)]
        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title == Description)
            {
                yield return new ValidationResult(
                    "Description should be different from Title",
                    new[] { nameof(CourseCreateDto) }
                );
            }
        }
    }
}
