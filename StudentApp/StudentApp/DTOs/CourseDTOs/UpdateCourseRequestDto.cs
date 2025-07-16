using System.ComponentModel.DataAnnotations;

namespace StudentApp.DTOs.CourseDTOs
{
    public class UpdateCourseRequestDto
    {
        [Required]
        public string CourseTitle { get; set; }
        [Required]
        public decimal Fee { get; set; }
        [Required]
        public int Credit { get; set; }
    }
}
