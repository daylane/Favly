using Favly.Application.Abstractions.Persistence;
using Favly.Application.Auth.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Auth.Commands.Login
{
    public class LoginHandler
    {
        public static async Task<LoginResponse> Handle(
            LoginCommand command,
            IUsuarioRepository repository,
            IGrupoRepository grupoRepository,
            IPasswordHasher hasher,
            ITokenService tokenService,
            CancellationToken ct)
        {
            var usuario = await repository.ObterPorEmailAsync(command.Email, ct);

            DomainException.When(usuario is null, "E-mail ou senha inválidos.");
            DomainException.When(!usuario!.Ativo, "Conta não ativada. Verifique seu e-mail.");
            DomainException.When(!hasher.Verificar(command.Senha, usuario.Hash), "E-mail ou senha inválidos.");

            var grupo = await grupoRepository.ObterGrupoPorUsuarioIdAsync(usuario.Id, ct);

            DomainException.When(grupo is null, "Usuário não possui grupo vinculado.");

            var token = tokenService.GerarToken(usuario);

            return new LoginResponse(
                Token: token,
                Nome: usuario.Nome,
                Email: usuario.Email.EnderecoEmail,
                GrupoId: grupo.Id,
                GrupoNome: grupo.Nome,
                Expiracao: DateTime.UtcNow.AddDays(1));
        }
    }
}
