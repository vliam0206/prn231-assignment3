using BusinessObjects;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarRentingWebClient.Models;

public class RegisterModel : Customer
{
    [Required(ErrorMessage = "Confirm password is required!")]
    [DisplayName("Confirm password")]
    [Compare("Password", ErrorMessage = "Confirm password does not match the password.")]
    public string ConfirmPassword { get; set; } = null!;
}
