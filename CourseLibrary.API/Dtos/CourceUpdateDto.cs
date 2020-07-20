using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Dtos
{
    public class CourseUpdateDto
    {
        [Required(ErrorMessage = "Title should be not empty")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description should be not empty")]
        [MaxLength(1500)]
        public string Description { get; set; }
    }
}
