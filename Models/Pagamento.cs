using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestauranteApp.Models
{
    public class Pagamento
    {
        public int IdPagamento { get; set; }

        [Required]
        [Display(Name = "Reserva")]
        public int IdReserva { get; set; }
        public Reserva? Reserva { get; set; }

        [Required]
        [Display(Name = "Método de Pagamento")]
        public int IdMetodo { get; set; }
        public MetodoPagamento? MetodoPagamento { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Valor")]
        public decimal Valor { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "pendente";

        [Display(Name = "Data do Pagamento")]
        public DateTime? DataPagamento { get; set; }
    }
}
