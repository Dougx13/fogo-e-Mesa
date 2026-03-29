using System.ComponentModel.DataAnnotations;

namespace RestauranteApp.Models
{
    public class Reserva
    {
        public int IdReserva { get; set; }

        [Required(ErrorMessage = "Data é obrigatória")]
        [DataType(DataType.Date)]
        [Display(Name = "Data da Reserva")]
        public DateTime DataReserva { get; set; }


        [Required(ErrorMessage = "Horário é obrigatório")]
        [DataType(DataType.Time)]
        [Display(Name = "Horário")]
        public TimeSpan Horario { get; set; }

        [Required(ErrorMessage = "Quantidade de pessoas é obrigatória")]
        [Range(1, 50)]
        [Display(Name = "Qtd. Pessoas")]
        public int QuantidadePessoas { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "pendente"; 

        [Required]
        [Display(Name = "Cliente")]
        public int IdCliente { get; set; }
        public Cliente? Cliente { get; set; }

        [Required]
        [Display(Name = "Mesa")]
        public int IdMesa { get; set; }
        public Mesa? Mesa { get; set; }

        public Pagamento? Pagamento { get; set; }

        [StringLength(500)]
        [Display(Name = "Pedido Antecipado")]
        public string? PedidoReserva { get; set; }
    }
}
