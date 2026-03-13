using atividade_4._1.Settings;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;

namespace atividade_4._1.Services.StorageService;

public class GoogleCloudStorageService : IStorageService
{
    private readonly StorageClient _storageClient;
    private readonly GoogleCloudSettings _options;

    public GoogleCloudStorageService(IOptions<GoogleCloudSettings> options)
    {
        _storageClient = StorageClient.Create();
        _options = options.Value;
    }

    public async Task<string> SalvarArquivo(Stream arquivo, string nomeArquivo, string contentType)
    {
        await _storageClient.UploadObjectAsync(
            _options.BucketName,
            nomeArquivo,
            contentType,
            arquivo);

        return nomeArquivo;
    }

    public async Task RemoverArquivo(string caminhoArquivo)
    {
        await _storageClient.DeleteObjectAsync(_options.BucketName, caminhoArquivo);
    }

    public Task<List<string>> ListarArquivos(string pastaArquivos)
    {
        var objetos = _storageClient.ListObjects(_options.BucketName, pastaArquivos);

        var urls = objetos
            .Where(o => !o.Name.EndsWith("/"))
            .Select(o => $"{_options.StorageBaseUrl}/{_options.BucketName}/{o.Name}")
            .ToList();

        return Task.FromResult(urls);
    }

    public string GetBaseUrl()
    {
        return $"{_options.StorageBaseUrl}/{_options.BucketName}";
    }
}