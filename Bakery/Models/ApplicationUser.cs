using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Bakery.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Please enter your first and last name.")]
        public string DisplayName { get; set;}
    }
}