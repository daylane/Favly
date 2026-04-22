using Favly.Application.Abstractions.Persistence;
using Favly.Application.Grupos.DTOs;
using Favly.Domain.Common.Enums;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;

namespace Favly.Application.Convites.Commands.AceitarConvite
{
    public class AceitarConviteHandler
    {
        public static async Task<GrupoResponse> Handle(
            AceitarConviteCommand command,
            IConviteRepository conviteRepository,
            IGrupoRepository grupoRepository,
            IUsuarioRepository usuarioRepository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var convite = await conviteRepository.ObterPorCodigoAsync(command.Codigo, ct);
            NotFoundException.When(convite is null, "Convite não encontrado.");

            var usuario = await usuarioRepository.ObterPorIdAsync(command.UsuarioId, ct);
            NotFoundException.When(usuario is null, "Usuário não encontrado.");

            DomainException.When(
                convite!.EmailConvidado.EnderecoEmail != usuario!.Email.EnderecoEmail,
                "Este convite pertence a outro e-mail.");

            var grupo = await grupoRepository.ObterPorIdComMembrosAsync(convite.FamiliaId, ct);
            NotFoundException.When(grupo is null, "Grupo não encontrado.");

            convite.Aceitar();
            grupo!.AdicionarMembro(command.UsuarioId, command.Apelido, PapelMembro.Usuario);

            conviteRepository.Atualizar(convite);
            grupoRepository.AtualizarAsync(grupo);
            await uow.CommitAsync(ct);

            return GrupoResponse.FromEntity(grupo);
        }
    }
}
