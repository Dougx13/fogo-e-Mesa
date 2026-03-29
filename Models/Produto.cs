using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestauranteApp.Models
{
    public class Produto
    {
        public int IdProduto { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Preço é obrigatório")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, 9999.99, ErrorMessage = "Preço deve ser maior que zero")]
        [Display(Name = "Preço")]
        public decimal Preco { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal QTD_Produtos { get; set; }

        [Required]
        [Display(Name = "Categoria")]
        public int IdCategoria { get; set; }
        public Categoria? Categoria { get; set; }

    
        
    }
}
