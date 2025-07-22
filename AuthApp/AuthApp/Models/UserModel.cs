using System.ComponentModel.DataAnnotations;

namespace AuthApp.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required]
        public string Userame { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
