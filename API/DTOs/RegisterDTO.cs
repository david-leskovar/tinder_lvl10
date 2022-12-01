using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDTO 
    {
        [Required]
        [MinLength(5)]
        public string? username { get; set; }

        [Required]
        [MinLength(5)]

        public string? password { get; set; }
     
    }
}
