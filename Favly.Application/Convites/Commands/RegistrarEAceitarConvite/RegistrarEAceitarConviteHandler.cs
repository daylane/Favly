using Favly.Application.Abstractions.Persistence;
using Favly.Application.Convites.DTOs;
using Favly.Domain.Common.Enums;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;

namespace Favly.Application.Convites.Commands.RegistrarEAceitarConvite
{
    public class RegistrarEAceitarConviteHandler
    {
        public static async Task<RegistrarEAceitarResponse> Handle(
            RegistrarEAceitarConviteCommand command,
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

            // 2. Garante que o e-mail ainda não tem cadastro
            //    (usuário existente deve usar o fluxo POST /convites/{codigo}/aceitar)
            var jaExiste = await usuarioRepository.EmailExisteAsync(convite.EmailConvidado.EnderecoEmail, ct);
            DomainException.When(jaExiste,
                "Este e-mail já possui uma conta. Faça login e use o link do convite para entrar no grupo.");

            // 3. Cria o usuário e ativa imediatamente
            //    (o e-mail foi verificado ao clicar no link do convite)
            var hash = hasher.Hash(command.Senha);
            var usuario = Usuario.Criar(
                convite.EmailConvidado.EnderecoEmail,
                command.Nome,
                hash,
                command.Avatar);
            usuario.Ativar(usuario.CodigoAtivacao);

            // 4. Adiciona ao grupo
            var grupo = await grupoRepository.ObterPorIdComMembrosAsync(convite.FamiliaId, ct);
            NotFoundException.When(grupo is null, "Grupo não encontrado.");
            grupo!.AdicionarMembro(usuario.Id, command.Apelido, PapelMembro.Usuario);

            // 5. Aceita o convite
            convite.Aceitar();

            // 6. Persiste tudo
            // convite foi carregado via query → já rastreado; change tracker detecta
            // a mudança de Status automaticamente. Não chamar Atualizar() para evitar
            // que Update() marque EmailConvidado (OwnsOne) como Modified separadamente.
            await usuarioRepository.AdicionarAsync(usuario, ct);
            grupoRepository.AtualizarAsync(grupo);
            await uow.CommitAsync(ct);

            // 7. Gera o token JWT para logar o usuário diretamente
            var token = tokenService.GerarToken(usuario);

            return new RegistrarEAceitarResponse(
                Token: token,
                UsuarioId: usuario.Id,
                UsuarioNome: usuario.Nome,
                UsuarioEmail: usuario.Email.EnderecoEmail,
                GrupoId: grupo.Id,
                GrupoNome: grupo.Nome,
                Expiracao: DateTime.UtcNow.AddDays(1));
        }
    }
}
