using Favly.Domain.Entities;

namespace Favly.Application.Grupos.DTOs
{
    public record GrupoResponse(
        Guid Id,
        string Nome,
        string Avatar,
        string CodigoConvite,
        int TotalMembros)
    {
        public static GrupoResponse FromEntity(Grupo g) => new(
            g.Id,
            g.Nome,
            g.Avatar,
            g.Convite,
            g.Membros.Count);
    }
}
