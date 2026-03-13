namespace atividade_4._1.Entities;

public sealed class Produto : Entitidade
{
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public decimal Preco { get; private set; }
    public string Categoria { get; private set; }

    public List<Imagem> Images { get; private set; } = new List<Imagem>();

    private Produto()
    {

    }

    public Produto(string nome, string descricao, decimal preco, string categoria)
    {
        Nome = nome;
        Descricao = descricao;
        Preco = preco;
        Categoria = categoria;
    }
}
