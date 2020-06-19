using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {  
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(9, MinimumLength=4, ErrorMessage ="You must specify password between 4 and 9 characters")]
        public string Password { get; set; }

        [Required]
        public string Gender { get; set; }
        [Required]
        public string  KnownAs { get; set; }
        [Required]
        public DateTime DateofBirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }

        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        // Adding constructor because not capturing the last two dates in register form
        public UserForRegisterDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}