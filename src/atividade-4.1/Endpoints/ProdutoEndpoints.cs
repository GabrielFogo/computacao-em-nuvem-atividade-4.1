using atividade_4._1.Requests;
using atividade_4._1.Services.Produtos;

namespace atividade_4._1.Endpoints;

public static class ProdutoEnpoints
{
    /// <summary>
    /// Responsável por registrar todos os endpoints relacionados a produtos.
    /// Os endpoints são agrupados sob a rota base "produtos".
    /// </summary>
    public static void MapProdutoEndpoints(this IEndpointRouteBuilder app)
    {
        // Cria um grupo de rotas com o prefixo "produtos" com o filtro pra tratar exceptions
        var api = app.MapGroup("produtos").AddEndpointFilter<ApiExceptionFilter>();

        /// <summary>
        /// Endpoint responsável por listar todos os produtos cadastrados.
        /// </summary>
        api.MapGet("/", async (IProdutoService produtoService) =>
        {
            var produtos = await produtoService.ListarProdutos();

            return Results.Ok(produtos);
        });

        /// <summary>
        /// Endpoint responsável por criar um novo produto.
        /// Recebe os dados do produto através do request body.
        /// </summary>
        api.MapPost("/", async (IProdutoService produtoService, CriarProdutoRequest produto) =>
        {
            await produtoService.CriarProduto(produto);

            return Results.Created();
        });

        /// <summary>
        /// Endpoint responsável por adicionar uma imagem a um produto específico.
        /// Recebe o identificador do produto na rota e o arquivo da imagem via multipart/form-data.
        /// </summary>
        api.MapPost("/{id:guid}/imagens", async (Guid id, IFormFile arquivo, IProdutoService produtoService) =>
        {
            await produtoService.InserirImagem(id, arquivo);

            return Results.NoContent();
        })
        // Desabilita a validação de antiforgery pois o endpoint recebe upload de arquivo
        .DisableAntiforgery();

        /// <summary>
        /// Endpoint responsável por listar todas as imagens associadas a um produto.
        /// Com a url é possivel acessar a imagem no bucket
        /// </summary>
        api.MapGet("/{id:guid}/imagens", async (Guid id, IProdutoService produtoService) =>
        {
            var imagens = await produtoService.ListarImagens(id);

            return Results.Ok(imagens);
        });

        /// <summary>
        /// Endpoint responsável por remover uma imagem específica de um produto.
        /// O identificador da imagem é informado na rota.
        /// </summary>
        api.MapDelete("/imagens/{id:guid}", async (Guid id, IProdutoService produtoService) =>
        {
            await produtoService.RemoverImage(id);

            return Results.NoContent();
        });
    }
}