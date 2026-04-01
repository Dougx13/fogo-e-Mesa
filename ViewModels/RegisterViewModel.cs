using System.ComponentModel.DataAnnotations;

namespace RestauranteApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(60, MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirme a senha")]
        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmarSenha { get; set; } = string.Empty;
    }
}
