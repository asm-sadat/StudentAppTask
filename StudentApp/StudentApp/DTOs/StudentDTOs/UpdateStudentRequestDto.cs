using System.ComponentModel.DataAnnotations;

namespace StudentApp.DTOs.StudentDTOs
{
    public class UpdateStudentRequestDto
    {
        [Required]
        public string Name { get; set; }
        public int? Age { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
