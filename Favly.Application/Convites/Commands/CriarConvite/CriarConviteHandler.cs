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
            IEmailService emailService,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(command.GrupoId, command.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var grupo = await grupoRepository.ObterPorIdAsync(command.GrupoId, ct);
            NotFoundException.When(grupo is null, "Grupo não encontrado.");

            var emailConvidado = EmailUsuario.Criar(command.EmailConvidado);

            var pendente = await conviteRepository.ObterPendentePorEmailEGrupoAsync(
                command.GrupoId, emailConvidado.EnderecoEmail, ct);
            DomainException.When(pendente is not null,
                "Já existe um convite pendente para este e-mail. Reenvie o convite existente ou aguarde sua expiração.");

            var convite = new Convite(command.GrupoId, emailConvidado);

            await conviteRepository.AdicionarAsync(convite, ct);
            await uow.CommitAsync(ct);

            await emailService.EnviarConviteAsync(
                email: emailConvidado.EnderecoEmail,
                grupoNome: grupo!.Nome,
                codigo: convite.Codigo,
                ct: ct);

            return ConviteResponse.FromEntity(convite);
        }
    }
}
