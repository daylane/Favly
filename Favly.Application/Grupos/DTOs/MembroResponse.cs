namespace Favly.Application.Grupos.DTOs
{
    public record MembroResponse(
        Guid Id,
        Guid UsuarioId,
        string Nome,
        string Avatar,
        string Apelido,
        string Role,
        DateTime DataEntrada);
}
