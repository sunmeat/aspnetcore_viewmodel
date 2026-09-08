using System.ComponentModel.DataAnnotations;

namespace StoringPassword.ViewModels
{
    // клас моделі-подання (view-model)
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Логін")]
        public string? Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }
    }
}