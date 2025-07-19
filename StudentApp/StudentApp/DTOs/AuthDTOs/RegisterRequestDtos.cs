using System.ComponentModel.DataAnnotations;

namespace StudentApp.DTOs.AuthDTOs
{
    public class RegisterRequestDtos
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType (DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string[] Roles { get; set; }
    }
}
