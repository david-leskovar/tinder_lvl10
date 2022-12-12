using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class LoginDTO
    {

        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string? Username { get; set; }


        [Required]
        [MinLength(5)]
        [MaxLength(15)]

        public string? password { get; set; }
    }
}
