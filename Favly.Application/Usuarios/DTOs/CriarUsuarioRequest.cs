namespace Favly.Application.Usuarios.DTOs
{
    public record CriarUsuarioRequest(
        string Nome,
        string Email,
        string Senha,
        string? Avatar = null);
}
