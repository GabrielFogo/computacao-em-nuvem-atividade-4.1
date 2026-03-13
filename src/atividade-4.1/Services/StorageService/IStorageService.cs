namespace atividade_4._1.Services.StorageService
{
    public interface IStorageService
    {
        public string GetBaseUrl();
        Task<string> SalvarArquivo(Stream arquivo, string nomeArquivo, string contentType);
        Task RemoverArquivo(string caminhoArquivo);
        Task<List<string>> ListarArquivos(string pastaArquivos);

    }
}
