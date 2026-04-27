using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Auth.Commands.RedefinirSenha
{
    public class RedefinirSenhaHandler
    {
        public static async Task Handle(
          RedefinirSenhaCommand command,
          ITokenResetSenhaRepository tokenRepository,
          IUsuarioRepository usuarioRepository,
          IPasswordHasher hasher,
          IUnitOfWork uow,
          CancellationToken ct)
        {
            var tokenReset = await tokenRepository.ObterPorTokenAsync(command.Token, ct);

            NotFoundException.When(tokenReset is null, "Token inválido ou expirado.");

            tokenReset!.Validar(); // lança DomainException se expirado ou já usado

            var usuario = await usuarioRepository.ObterPorIdAsync(tokenReset.UsuarioId, ct);

            NotFoundException.When(usuario is null, "Usuário não encontrado.");

            var novoHash = hasher.Hash(command.NovaSenha);
            usuario!.RedefinirSenha(novoHash);

            tokenReset.Usar();

            usuarioRepository.Atualizar(usuario);
            tokenRepository.Atualizar(tokenReset);

            await uow.CommitAsync(ct);
        }
    }
}
