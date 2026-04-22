using Favly.Domain.Entities;

namespace Favly.Application.Usuarios.DTOs
{
    public record UsuarioResponse(
        Guid Id,
        string Nome,
        string Email,
        string Avatar,
        bool Ativo,
        DateTime DataCriacao)
    {
        public static UsuarioResponse FromEntity(Usuario usuario) => new(
            usuario.Id,
            usuario.Nome,
            usuario.Email.EnderecoEmail,
            usuario.Avatar,
            usuario.Ativo,
            usuario.DataCriacao);
    }
}
