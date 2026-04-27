using Favly.Application.Abstractions.Persistence;
using Favly.Domain.Common.Enums;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;

namespace Favly.Application.Grupos.Commands.SairDoGrupo
{
    public class SairDoGrupoHandler
    {
        public static async Task Handle(
            SairDoGrupoCommand command,
            IGrupoRepository grupoRepository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var grupo = await grupoRepository.ObterPorIdComMembrosAsync(command.GrupoId, ct);
            NotFoundException.When(grupo is null, "Grupo não encontrado.");

            // Guarda o apelido antes de remover o membro (usado no grupo pessoal)
            var membro = grupo!.Membros.FirstOrDefault(m => m.UsuarioId == command.UsuarioId);
            NotFoundException.When(membro is null, "Você não é membro deste grupo.");

            var apelido = membro!.Apelido;

            // Se este é o último grupo do usuário, cria um grupo pessoal antes de sair
            // para garantir que o usuário nunca fique sem grupo.
            var possuiOutros = await grupoRepository.UsuarioPossuiOutrosGruposAsync(
                command.UsuarioId, command.GrupoId, ct);

            if (!possuiOutros)
            {
                var grupoPessoal = new Grupo($"Grupo de {apelido}");
                grupoPessoal.AdicionarMembro(command.UsuarioId, apelido, PapelMembro.Administrador);
                await grupoRepository.AdicionarAsync(grupoPessoal, ct);
            }

            // Regras de domínio ao sair:
            // - único membro → não permite (grupo ficaria vazio)
            // - admin saindo com outros membros → promove o mais antigo a admin
            var membroRemovido = grupo.SairDoGrupo(command.UsuarioId);
            grupoRepository.RemoverMembro(membroRemovido);

            await uow.CommitAsync(ct);
        }
    }
}
