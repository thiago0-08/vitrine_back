namespace Vitrine.DTO
{
    public class ProdutoDTO
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public double Preco { get; set; }
        public string[] Tamanhos { get; set; }
        public string[] Cores { get; set; }
        public string Imagem { get; set; }
        public int IdCategoria { get; set; }
    }
}
