using System.ComponentModel.DataAnnotations;

namespace Bakery.ViewModels
{
  public class RegisterViewModel
  {
    [Required]
    [EmailAddress]
    [Display(Name = "Email Address")]
    public string Email { get; set; }

     [Required]
    [Display(Name = "First and Last Name")]
    public string DisplayName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
  }
}