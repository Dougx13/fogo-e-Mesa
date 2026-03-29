
using System.ComponentModel.DataAnnotations;

namespace RestauranteApp.Models
{
    public class Categoria
    {
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(50)]
        public string Nome { get; set; } = string.Empty;

       
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}
