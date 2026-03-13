using atividade_4._1.Requests;
using atividade_4._1.Services.Produtos;

namespace atividade_4._1.Endpoints;

public static class ProdutoEnpoints
{
    public static void MapProdutoEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("produtos");

        api.MapGet("/", async (IProdutoService produtoService) =>
        {
            var produtos = await produtoService.ListarProdutos();

            return Results.Ok(produtos);
        });

        api.MapPost("/", async (IProdutoService produtoService, CriarProdutoRequest produto) =>
        {
            await produtoService.CriarProduto(produto);

            return Results.Created();
        });

        api.MapPost("/{id:guid}/imagens", async (Guid id, IFormFile arquivo, IProdutoService produtoService) =>
        {
            await produtoService.InserirImagem(id, arquivo);

            return Results.NoContent();
        }).DisableAntiforgery();


        api.MapGet("/{id:guid}/imagens", async (Guid id, IProdutoService produtoService) =>
        {
            var imagens = await produtoService.ListarImagens(id);

            return Results.Ok(imagens);
        });

        api.MapDelete("/imagens/{id:guid}", async (Guid id, IProdutoService produtoService) =>
        {
            await produtoService.RemoverImage(id);

            return Results.NoContent();
        });
    }
}
