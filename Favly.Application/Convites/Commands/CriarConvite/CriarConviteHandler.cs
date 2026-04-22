using Favly.Application.Abstractions.Persistence;
using Favly.Application.Convites.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using Favly.Domain.ValueObjects;

namespace Favly.Application.Convites.Commands.CriarConvite
{
    public class CriarConviteHandler
    {
        public static async Task<ConviteResponse> Handle(
            CriarConviteCommand command,
            IConviteRepository conviteRepository,
            IGrupoRepository grupoRepository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(command.GrupoId, command.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var emailConvidado = EmailUsuario.Criar(command.EmailConvidado);
            var convite = new Convite(command.GrupoId, emailConvidado);

            await conviteRepository.AdicionarAsync(convite, ct);
            await uow.CommitAsync(ct);

            return ConviteResponse.FromEntity(convite);
        }
    }
}
