using atividade_4._1.Settings;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;

namespace atividade_4._1.Services.StorageService;

/// <summary>
/// Implementação do serviço de armazenamento utilizando o Google Cloud Storage.
/// Responsável por realizar operações de upload, remoção e listagem de arquivos
/// dentro de um bucket configurado.
/// </summary>
public class GoogleCloudStorageService : IStorageService
{
    private readonly StorageClient _storageClient;
    private readonly GoogleCloudSettings _options;

    /// <summary>
    /// Construtor que inicializa o cliente do Google Cloud Storage
    /// e carrega as configurações definidas na aplicação.
    /// </summary>
    public GoogleCloudStorageService(IOptions<GoogleCloudSettings> options)
    {
        _storageClient = StorageClient.Create();
        _options = options.Value;
    }

    /// <summary>
    /// Realiza o upload de um arquivo para o bucket configurado.
    /// </summary>
    /// <param name="arquivo">Stream contendo o arquivo a ser enviado.</param>
    /// <param name="nomeArquivo">Caminho/nome que o arquivo terá dentro do bucket.</param>
    /// <param name="contentType">Tipo MIME do arquivo.</param>
    /// <returns>Retorna o caminho do arquivo armazenado.</returns>
    public async Task<string> SalvarArquivo(Stream arquivo, string nomeArquivo, string contentType)
    {
        await _storageClient.UploadObjectAsync(
            _options.BucketName,
            nomeArquivo,
            contentType,
            arquivo);

        return nomeArquivo;
    }

    /// <summary>
    /// Remove um arquivo específico do bucket.
    /// </summary>
    /// <param name="caminhoArquivo">Caminho do arquivo dentro do bucket.</param>
    public async Task RemoverArquivo(string caminhoArquivo)
    {
        await _storageClient.DeleteObjectAsync(_options.BucketName, caminhoArquivo);
    }

    /// <summary>
    /// Lista todos os arquivos existentes dentro de uma pasta do bucket.
    /// </summary>
    /// <param name="pastaArquivos">Prefixo/pasta onde os arquivos estão armazenados.</param>
    /// <returns>Lista contendo as URLs completas dos arquivos.</returns>
    public Task<List<string>> ListarArquivos(string pastaArquivos)
    {
        var objetos = _storageClient.ListObjects(_options.BucketName, pastaArquivos);

        var urls = objetos
            // Ignora "pastas" virtuais criadas no bucket
            .Where(o => !o.Name.EndsWith("/"))
            // Monta a URL pública do arquivo
            .Select(o => $"{_options.StorageBaseUrl}/{_options.BucketName}/{o.Name}")
            .ToList();

        return Task.FromResult(urls);
    }

    /// <summary>
    /// Retorna a URL base do bucket configurado.
    /// Utilizada para montar URLs públicas dos arquivos armazenados.
    /// </summary>
    public string GetBaseUrl()
    {
        return $"{_options.StorageBaseUrl}/{_options.BucketName}";
    }
}