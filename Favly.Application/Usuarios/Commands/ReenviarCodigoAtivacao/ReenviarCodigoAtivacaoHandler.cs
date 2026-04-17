using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Wolverine;

namespace Favly.Application.Usuarios.Commands.ReenviarCodigoAtivacao
{
    public class ReenviarCodigoAtivacaoHandler
    {
        public static async Task Handle(
        ReenviarCodigoAtivacaoCommand command,
        IUsuarioRepository repository,
        IEmailService emailService,
        IUnitOfWork uow,
        CancellationToken ct)
        {
            var usuario = await repository.ObterPorEmailAsync(command.Email, ct);

            NotFoundException.When(usuario is null, "Usuário não encontrado.");
            DomainException.When(usuario!.Ativo, "Esta conta já está ativa.");

            usuario.GerarCodigo();
            repository.Atualizar(usuario);
            await uow.CommitAsync(ct);

            await emailService.EnviarCodigoAtivacaoAsync(
                email: usuario.Email.EnderecoEmail,
                nome: usuario.Nome,
                codigo: usuario.CodigoAtivacao,
                ct: ct);
        }
    }
}
