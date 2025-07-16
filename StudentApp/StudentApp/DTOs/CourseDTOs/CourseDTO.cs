using System.ComponentModel.DataAnnotations;

namespace StudentApp.DTOs.CourseDTOs
{
    public class CourseDTO
    {
        public int Id { get; set; }
        [Required]
        public string CourseTitle { get; set; }
        [Required]
        public decimal Fee { get; set; }
        [Required]
        public int Credit { get; set; }
    }
}
