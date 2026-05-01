using Favly.Application.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Favly.Infrastructure.Services
{
    public class ArquivoService(IConfiguration config) : IArquivoService
    {
        private static readonly HashSet<string> _extensoesPermitidas = [".jpg", ".jpeg", ".png", ".webp", ".gif"];

        private string WebRootPath =>
            config["App:WebRootPath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        private string BaseUrl =>
            config["App:BaseUrl"]?.TrimEnd('/') ?? string.Empty;

        public async Task<string> SalvarAvatarAsync(Stream conteudo, string extensao, CancellationToken ct = default)
        {
            extensao = extensao.ToLowerInvariant();

            if (!_extensoesPermitidas.Contains(extensao))
                throw new InvalidOperationException($"Extensão '{extensao}' não permitida. Use: jpg, png, webp ou gif.");

            var pasta = Path.Combine(WebRootPath, "uploads", "avatars");
            Directory.CreateDirectory(pasta);

            var nomeArquivo = $"{Guid.NewGuid()}{extensao}";
            var caminhoFisico = Path.Combine(pasta, nomeArquivo);

            await using var arquivo = File.Create(caminhoFisico);
            await conteudo.CopyToAsync(arquivo, ct);

            return $"{BaseUrl}/uploads/avatars/{nomeArquivo}";
        }

        public Task RemoverAsync(string url, CancellationToken ct = default)
        {
            var caminho = url.Replace(BaseUrl, string.Empty)
                             .TrimStart('/')
                             .Replace('/', Path.DirectorySeparatorChar);

            var caminhoFisico = Path.Combine(WebRootPath, caminho);

            if (File.Exists(caminhoFisico))
                File.Delete(caminhoFisico);

            return Task.CompletedTask;
        }
    }
}
