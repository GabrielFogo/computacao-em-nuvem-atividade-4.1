namespace atividade_4._1.Responses
{
    public sealed class ProdutoResponse
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public decimal Preco { get; private set; }
        public string Categoria { get; private set; }

        public ProdutoResponse(Guid id, string nome, string descricao, decimal preco, string categoria)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            Categoria = categoria;
        }
    }
}
