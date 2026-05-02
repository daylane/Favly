using Favly.Domain.Common.Enums;
using Favly.Domain.Entities;

namespace Favly.Application.Grupos.DTOs
{
    public record GrupoResponse(
        Guid Id,
        string Nome,
        string Avatar,
        string CodigoConvite,
        int TotalMembros,
        PapelMembro? MinhaRole)
    {
        public static GrupoResponse FromEntity(Grupo g, Guid usuarioId) => new(
            g.Id,
            g.Nome,
            g.Avatar,
            g.Convite,
            g.Membros.Count,
            g.Membros.FirstOrDefault(m => m.UsuarioId == usuarioId)?.Role);
    }
}
