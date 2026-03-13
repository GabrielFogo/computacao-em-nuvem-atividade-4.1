namespace atividade_4._1.Responses
{
    public sealed class ImageResponse
    {
        public Guid Id { get; private set; }
        public string Url { get; private set; }

        public ImageResponse(string url, Guid id)
        {
            Url = url;
            Id = id;
        }
    }
}
