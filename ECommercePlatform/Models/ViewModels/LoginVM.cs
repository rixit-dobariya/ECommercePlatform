using System.ComponentModel.DataAnnotations;

public class LoginVM
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 6 characters long.")]
    public string? Password { get; set; }

    public bool RememberMe { get; set; }
}