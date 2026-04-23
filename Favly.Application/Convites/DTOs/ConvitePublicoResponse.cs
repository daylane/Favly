namespace Favly.Application.Convites.DTOs
{
    public record ConvitePublicoResponse(
        Guid Id,
        Guid GrupoId,
        string GrupoNome,
        string GrupoAvatar,
        string EmailConvidado,
        DateTime DataExpiracao,
        bool UsuarioJaCadastrado);
}
