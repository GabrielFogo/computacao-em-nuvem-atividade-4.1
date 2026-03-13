using atividade_4._1.Context;
using atividade_4._1.Entities;
using atividade_4._1.Requests;
using atividade_4._1.Responses;
using atividade_4._1.Services.StorageService;
using Microsoft.EntityFrameworkCore;

namespace atividade_4._1.Services.Produtos;

public class ProdutoService : IProdutoService
{
    private readonly AplicacaoDbContext _context;
    private readonly IStorageService _storageService;

    public ProdutoService(AplicacaoDbContext context, IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    public async Task CriarProduto(CriarProdutoRequest request)
    {
        var produto = new Produto(request.Nome, request.Descricao, request.Price, request.Categoria);

        _context.Produtos.Add(produto);

        await _context.SaveChangesAsync();
    }

    public async Task InserirImagem(Guid produtoId, IFormFile arquivo)
    {
        using var stream = arquivo.OpenReadStream();
        var path = $"produtos/{produtoId}/{arquivo.FileName}";
        var imagemPath = await _storageService.SalvarArquivo(stream, path, arquivo.ContentType);
        var imagem = new Imagem(imagemPath, produtoId);

        _context.Imagens.Add(imagem);

        await _context.SaveChangesAsync();
    }

    public async Task<List<ProdutoResponse>> ListarProdutos()
    {
        return await _context.Produtos
            .Select(p => new ProdutoResponse(p.Id, p.Nome, p.Descricao, p.Preco, p.Categoria))
            .ToListAsync();
    }

    public async Task RemoverImage(Guid imagemId)
    {
        var imagem = await _context.Imagens.FirstOrDefaultAsync(i => i.Id == imagemId);

        await _storageService.RemoverArquivo(imagem.Path);

        _context.Imagens.Remove(imagem);

        await _context.SaveChangesAsync();
    }

    public async Task<List<ImageResponse>> ListarImagens(Guid produtoId)
    {
        // isso pega do sdk mas vou recuperar o caminho pelo banco de dados porque é melhor;
        //var urls = await _storageService.ListarArquivos($"produtos/{produtoId}");

        var storageBaseUrl = _storageService.GetBaseUrl();

        return await _context.Imagens
            .Select(i => new ImageResponse($"{storageBaseUrl}/{i.Path}", i.Id))
            .ToListAsync();
    }
}
