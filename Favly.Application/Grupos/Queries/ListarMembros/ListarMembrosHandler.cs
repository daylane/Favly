using Favly.Application.Abstractions.Persistence;
using Favly.Application.Grupos.DTOs;
using Favly.Domain.Common.Exceptions;

namespace Favly.Application.Grupos.Queries.ListarMembros
{
    public class ListarMembrosHandler
    {
        public static async Task<IEnumerable<MembroResponse>> Handle(
            ListarMembrosQuery query,
            IGrupoRepository grupoRepository,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(query.GrupoId, query.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var membros = await grupoRepository.ObterMembrosComUsuariosAsync(query.GrupoId, ct);

            return membros.Select(m => new MembroResponse(
                m.MembroId,
                m.UsuarioId,
                m.NomeUsuario,
                m.Avatar,
                m.Apelido,
                m.Role.ToString(),
                m.DataEntrada));
        }
    }
}
