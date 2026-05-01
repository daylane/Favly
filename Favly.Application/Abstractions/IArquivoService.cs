namespace Favly.Application.Abstractions
{
    public interface IArquivoService
    {
        /// <summary>
        /// Salva um arquivo e retorna a URL pública para acessá-lo.
        /// </summary>
        Task<string> SalvarAvatarAsync(Stream conteudo, string extensao, CancellationToken ct = default);

        /// <summary>
        /// Remove um arquivo pelo caminho relativo da URL.
        /// </summary>
        Task RemoverAsync(string url, CancellationToken ct = default);
    }
}
