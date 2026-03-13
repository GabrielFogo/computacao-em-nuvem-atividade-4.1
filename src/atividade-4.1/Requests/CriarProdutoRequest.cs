namespace atividade_4._1.Requests
{
    public sealed class CriarProdutoRequest
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public decimal Price { get; private set; }
        public string Categoria { get; private set; }

        public CriarProdutoRequest(string nome, string descricao, decimal price, string categoria)
        {
            Nome = nome;
            Descricao = descricao;
            Price = price;
            Categoria = categoria;
        }
    }
}
