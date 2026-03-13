namespace atividade_4._1.Entities;

public sealed class Imagem : Entitidade
{
    public string Path { get; private set; }
    public Guid ProdutoId { get; private set; }
    public Produto Produto { get; private set; }

    private Imagem()
    {

    }

    public Imagem(string path, Guid produtoId)
    {
        Path = path;
        ProdutoId = produtoId;
    }
}
