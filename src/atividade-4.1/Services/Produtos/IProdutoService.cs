using atividade_4._1.Requests;
using atividade_4._1.Responses;

namespace atividade_4._1.Services.Produtos;

public interface IProdutoService
{
    Task<List<ProdutoResponse>> ListarProdutos();
    Task CriarProduto(CriarProdutoRequest request);
    Task<List<ImageResponse>> ListarImagens(Guid produtoId);
    Task InserirImagem(Guid produtoId, IFormFile arquivo);
    Task RemoverImage(Guid imagemId);
}
