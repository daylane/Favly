using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Auth.Commands.EsquecerSenha
{
    public class EsqueciSenhaHandler
    {
        public static async Task Handle(
           EsqueciSenhaCommand command,
           IUsuarioRepository usuarioRepository,
           ITokenResetSenhaRepository tokenRepository,
           IEmailService emailService,
           IUnitOfWork uow,
           CancellationToken ct)
        {
            var usuario = await usuarioRepository.ObterPorEmailAsync(command.Email, ct);

            // Resposta genérica intencional — não revela se o email existe
            if (usuario is null || !usuario.Ativo)
                return;

            var token = TokenResetSenha.Criar(usuario.Id);
            await tokenRepository.AdicionarAsync(token, ct);
            await uow.CommitAsync(ct);

            await emailService.EnviarResetSenhaAsync(
                email: usuario.Email.EnderecoEmail,
                nome: usuario.Nome,
                token: token.Token,
                ct: ct);
        }
    }
}
