using System.ComponentModel.DataAnnotations;

namespace AquaDefender_Backend.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Numele complet este obligatoriu.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email-ul este obligatoriu.")]
        [EmailAddress(ErrorMessage = "Formatul adresei de email nu este valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Numărul de telefon este obligatoriu.")]
        [Phone(ErrorMessage = "Formatul numărului de telefon nu este valid.")]
        [StringLength(10, ErrorMessage = "Numărul de telefon trebuie să aibă 10 cifre.", MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Județul este obligatoriu.")]
        public string County { get; set; }

        [Required(ErrorMessage = "Localitatea este obligatorie.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie.")]
        [StringLength(100, ErrorMessage = "Parola trebuie să aibă peste 8 caractere.", MinimumLength = 8)]
        public string Password { get; set; }
    }
}
