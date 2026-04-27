namespace Favly.Application.Convites.DTOs
{
    public record RegistrarEAceitarResponse(
        string Token,
        Guid UsuarioId,
        string UsuarioNome,
        string UsuarioEmail,
        Guid GrupoId,
        string GrupoNome,
        DateTime Expiracao);
}
