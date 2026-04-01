using System.ComponentModel.DataAnnotations;

namespace RestauranteApp.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;
    }
}
