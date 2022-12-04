using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDTO 
    {
        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string? username { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(15)] 

        public string? password { get; set; }
     
    }
}
