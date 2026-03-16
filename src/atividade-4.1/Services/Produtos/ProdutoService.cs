using atividade_4._1.Context;
using atividade_4._1.Entities;
using atividade_4._1.Requests;
using atividade_4._1.Responses;
using atividade_4._1.Services.StorageService;
using Microsoft.EntityFrameworkCore;

namespace atividade_4._1.Services.Produtos;

/// <summary>
/// Serviço responsável por gerenciar as operações relacionadas a produtos
/// e suas respectivas imagens.
/// </summary>
public class ProdutoService : IProdutoService
{
    private readonly AplicacaoDbContext _context;
    private readonly IStorageService _storageService;

    /// <summary>
    /// Construtor que recebe as dependências necessárias para acesso ao banco de dados
    /// e ao serviço de armazenamento de arquivos.
    /// </summary>
    public ProdutoService(AplicacaoDbContext context, IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    /// <summary>
    /// Cria um novo produto no banco de dados a partir dos dados recebidos na requisição.
    /// </summary>
    public async Task CriarProduto(CriarProdutoRequest request)
    {
        var produto = new Produto(request.Nome, request.Descricao, request.Price, request.Categoria);

        _context.Produtos.Add(produto);

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Realiza o upload de uma imagem para o serviço de armazenamento
    /// e registra o caminho da imagem no banco de dados.
    /// </summary>
    public async Task InserirImagem(Guid produtoId, IFormFile arquivo)
    {
        // Abre o stream do arquivo enviado
        using var stream = arquivo.OpenReadStream();

        // Define o caminho onde o arquivo será armazenado
        var path = $"produtos/{produtoId}/{arquivo.FileName}";

        // Envia o arquivo para o serviço de armazenamento
        var imagemPath = await _storageService.SalvarArquivo(stream, path, arquivo.ContentType);

        // Cria a entidade de imagem associada ao produto
        var imagem = new Imagem(imagemPath, produtoId);

        _context.Imagens.Add(imagem);

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Retorna a lista de produtos cadastrados no sistema.
    /// Os dados são projetados diretamente para o objeto de resposta.
    /// </summary>
    public async Task<List<ProdutoResponse>> ListarProdutos()
    {
        return await _context.Produtos
            .Select(p => new ProdutoResponse(p.Id, p.Nome, p.Descricao, p.Preco, p.Categoria))
            .ToListAsync();
    }

    /// <summary>
    /// Remove uma imagem associada a um produto.
    /// A imagem é removida tanto do armazenamento quanto do banco de dados.
    /// </summary>
    public async Task RemoverImage(Guid imagemId)
    {
        var imagem = await _context.Imagens.FirstOrDefaultAsync(i => i.Id == imagemId);

        // Remove o arquivo do serviço de armazenamento
        await _storageService.RemoverArquivo(imagem.Path);

        // Remove o registro da imagem no banco de dados
        _context.Imagens.Remove(imagem);

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Retorna a lista de imagens associadas a um produto específico.
    /// A URL completa da imagem é montada utilizando a base do storage.
    /// </summary>
    public async Task<List<ImageResponse>> ListarImagens(Guid produtoId)
    {
        // Embora seja possível listar arquivos diretamente pelo SDK do storage,
        // a recuperação das imagens é feita via banco de dados para maior controle.

        // Obtém a URL base do serviço de armazenamento
        var storageBaseUrl = _storageService.GetBaseUrl();

        return await _context.Imagens
            .Select(i => new ImageResponse($"{storageBaseUrl}/{i.Path}", i.Id))
            .ToListAsync();
    }
}