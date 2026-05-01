using Favly.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Favly.api.Controllers
{
    [ApiController]
    [Route("api/uploads")]
    public class UploadsController(IArquivoService arquivoService) : ControllerBase
    {
        private const long MaxBytes = 5 * 1024 * 1024; // 5 MB

        /// <summary>
        /// Faz upload de um avatar e retorna a URL pública.
        /// Formatos aceitos: jpg, png, webp, gif. Tamanho máximo: 5 MB.
        /// </summary>
        [HttpPost("avatar")]
        [AllowAnonymous] // necessário no cadastro, antes de ter token
        [ProducesResponseType(typeof(UploadAvatarResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Avatar(IFormFile arquivo, CancellationToken ct)
        {
            if (arquivo is null || arquivo.Length == 0)
                return BadRequest("Nenhum arquivo enviado.");

            if (arquivo.Length > MaxBytes)
                return BadRequest("Arquivo muito grande. Tamanho máximo: 5 MB.");

            var extensao = Path.GetExtension(arquivo.FileName);
            await using var stream = arquivo.OpenReadStream();
            var url = await arquivoService.SalvarAvatarAsync(stream, extensao, ct);

            return Ok(new UploadAvatarResponse(url));
        }
    }

    public record UploadAvatarResponse(string Url);
}
