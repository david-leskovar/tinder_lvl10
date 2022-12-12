using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDTO 
    {
        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string? Username { get; set; }


        [Required]
        public  string KnownAs { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]

        public string City { get; set; }

        [Required]

        public string Country { get; set; }



        [Required]
        [MinLength(5)]
        [MaxLength(15)] 

        public string? password { get; set; }
     
    }
}
