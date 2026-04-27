using Favly.Application.Abstractions.Persistence;
using Favly.Application.Convites.DTOs;
using Favly.Domain.Common.Enums;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;

namespace Favly.Application.Convites.Commands.EntrarPorConvite
{
    public class EntrarPorConviteHandler
    {
        public static async Task<RegistrarEAceitarResponse> Handle(
            EntrarPorConviteCommand command,
            IConviteRepository conviteRepository,
            IGrupoRepository grupoRepository,
            IUsuarioRepository usuarioRepository,
            IPasswordHasher hasher,
            ITokenService tokenService,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            // 1. Valida o convite
            var convite = await conviteRepository.ObterPorCodigoAsync(command.Codigo, ct);
            NotFoundException.When(convite is null, "Convite não encontrado.");
            DomainException.When(convite!.Status != StatusConvite.Pendente, "Este convite já foi utilizado.");
            DomainException.When(convite.DataExpiracao < DateTime.UtcNow, "Este convite expirou.");

            // 2. Carrega o grupo
            var grupo = await grupoRepository.ObterPorIdComMembrosAsync(convite.FamiliaId, ct);
            NotFoundException.When(grupo is null, "Grupo não encontrado.");

            // 3. Decide pelo e-mail do convite: usuário existente ou novo
            var emailConvite = convite.EmailConvidado.EnderecoEmail;
            var usuarioExistente = await usuarioRepository.ObterPorEmailAsync(emailConvite, ct);

            Usuario usuario;

            if (usuarioExistente is not null)
            {
                // ── Fluxo: usuário já tem conta ──────────────────────────────
                DomainException.When(!usuarioExistente.Ativo, "Conta não ativada. Verifique seu e-mail.");
                DomainException.When(
                    !hasher.Verificar(command.Senha, usuarioExistente.Hash),
                    "Senha incorreta.");

                usuario = usuarioExistente;
            }
            else
            {
                // ── Fluxo: usuário novo ──────────────────────────────────────
                DomainException.When(
                    string.IsNullOrWhiteSpace(command.Nome),
                    "Informe seu nome para criar a conta.");

                var hash = hasher.Hash(command.Senha);
                usuario = Usuario.Criar(emailConvite, command.Nome!, hash, command.Avatar);

                // Conta ativada imediatamente: o e-mail foi verificado ao clicar no link
                usuario.Ativar(usuario.CodigoAtivacao);

                await usuarioRepository.AdicionarAsync(usuario, ct);
            }

            // 4. Adiciona ao grupo e aceita o convite
            grupo!.AdicionarMembro(usuario.Id, command.Apelido, PapelMembro.Usuario);
            convite.Aceitar();

            grupoRepository.AtualizarAsync(grupo);
            await uow.CommitAsync(ct);

            // 5. Devolve JWT para o front logar automaticamente
            var token = tokenService.GerarToken(usuario);

            return new RegistrarEAceitarResponse(
                Token: token,
                UsuarioId: usuario.Id,
                UsuarioNome: usuario.Nome,
                UsuarioEmail: usuario.Email.EnderecoEmail,
                GrupoId: grupo.Id,
                GrupoNome: grupo.Nome,
                Expiracao: DateTime.UtcNow.AddDays(7));
        }
    }
}
