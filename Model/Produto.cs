using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vitrine.Endpoints;

namespace Vitrine.Model
{
    public class Produto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id {  get; set; }
       
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public double Preco { get; set; }
        public string[] Tamanhos { get; set; }
        public string[] Cores { get; set; }
        public string Imagem { get; set; }
        public Categoria Categoria { get; set; }

    }
}
