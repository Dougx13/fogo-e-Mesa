using System.ComponentModel.DataAnnotations;

namespace RestauranteApp.Models
{
    public class Mesa
    {
        public int IdMesa { get; set; }

        [Required]
        [Display(Name = "Número da Mesa")]
        public int Numero { get; set; }

        [Required]
        [Display(Name = "Capacidade")]
        public int Capacidade { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Status")]
        public string Status { get; set; } = "disponivel"; 
        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
