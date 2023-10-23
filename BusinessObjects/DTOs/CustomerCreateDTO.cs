using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BusinessObjects.DTOs;

public class CustomerCreateDTO
{
    [Required(ErrorMessage = "Full name is required!")]
    [DisplayName("Full name")]
    public string CustomerName { get; set; } = null!;

    [Required(ErrorMessage = "Telephone is required!")]
    [MinLength(10, ErrorMessage = "Phone number at least 10 charaters.")]
    public string Telephone { get; set; } = null!;

    [Required(ErrorMessage = "Email is required!")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Birthday is required!")]
    [DisplayName("Birthday")]
    public DateTime CustomerBirthday { get; set; }

    [DisplayName("Status")]
    public byte? CustomerStatus { get; set; } = 1;

    [Required(ErrorMessage = "Password is required!")]
    public string Password { get; set; } = null!;
}
