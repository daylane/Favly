using Favly.Application.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Favly.Infrastructure.Services
{
    public class ArquivoService(IWebHostEnvironment env, IConfiguration config) : IArquivoService
    {
        private static readonly HashSet<string> _extensoesPermitidas = [".jpg", ".jpeg", ".png", ".webp", ".gif"];

        public async Task<string> SalvarAvatarAsync(Stream conteudo, string extensao, CancellationToken ct = default)
        {
            extensao = extensao.ToLowerInvariant();

            if (!_extensoesPermitidas.Contains(extensao))
                throw new InvalidOperationException($"Extensão '{extensao}' não permitida. Use: jpg, png, webp ou gif.");

            var pasta = Path.Combine(env.WebRootPath, "uploads", "avatars");
            Directory.CreateDirectory(pasta);

            var nomeArquivo = $"{Guid.NewGuid()}{extensao}";
            var caminhoFisico = Path.Combine(pasta, nomeArquivo);

            await using var arquivo = File.Create(caminhoFisico);
            await conteudo.CopyToAsync(arquivo, ct);

            var baseUrl = config["App:BaseUrl"]?.TrimEnd('/') ?? string.Empty;
            return $"{baseUrl}/uploads/avatars/{nomeArquivo}";
        }

        public Task RemoverAsync(string url, CancellationToken ct = default)
        {
            // Extrai o caminho relativo da URL e apaga o arquivo
            var baseUrl = config["App:BaseUrl"]?.TrimEnd('/') ?? string.Empty;
            var caminho = url.Replace(baseUrl, string.Empty).TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var caminhoFisico = Path.Combine(env.WebRootPath, caminho);

            if (File.Exists(caminhoFisico))
                File.Delete(caminhoFisico);

            return Task.CompletedTask;
        }
    }
}
