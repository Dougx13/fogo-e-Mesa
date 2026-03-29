using System.ComponentModel.DataAnnotations;

namespace RestauranteApp.Models
{
    public class MetodoPagamento
    {
        public int IdMetodo { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(50)]
        [Display(Name = "Método de Pagamento")]
        public string Nome { get; set; } = string.Empty; 

        public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
    }
}
