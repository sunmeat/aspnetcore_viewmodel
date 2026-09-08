using System.ComponentModel.DataAnnotations;

namespace StoringPassword.ViewModels
{
    // клас моделі-подання (view-model)
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Ім'я")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Прізвище")]
        public string? LastName { get; set; }

        [Required]
        [Display(Name = "Логін")]
        public string? Login { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [Display(Name = "Підтвердження пароля")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [DataType(DataType.Password)]
        public string? PasswordConfirm { get; set; }
    }
}