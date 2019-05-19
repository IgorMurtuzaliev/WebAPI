using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCore.Models
{
    public class User : IdentityUser
    {

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Input you name")]
        public string Name { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "Input you surname")]
        public string LastName { get; set; }

        [Display(Name = "Register date")]
        [DataType(DataType.DateTime)]
        public DateTime DateOfRegister { get; set; } = DateTime.Now;
        public string Avatar { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public User()
        {

        }
    }
}
