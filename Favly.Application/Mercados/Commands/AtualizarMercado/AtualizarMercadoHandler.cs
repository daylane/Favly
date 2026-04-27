using Favly.Application.Abstractions.Persistence;
using Favly.Application.Mercados.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;

namespace Favly.Application.Mercados.Commands.AtualizarMercado
{
    public class AtualizarMercadoHandler
    {
        public static async Task<MercadoResponse> Handle(
            AtualizarMercadoCommand command,
            IMercadoRepository repository,
            IGrupoRepository grupoRepository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(command.GrupoId, command.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var mercado = await repository.ObterPorIdAsync(command.MercadoId, ct);
            NotFoundException.When(mercado is null, "Mercado não encontrado.");

            mercado!.Atualizar(command.Nome, command.Endereco);
            repository.Atualizar(mercado);
            await uow.CommitAsync(ct);

            return MercadoResponse.FromEntity(mercado);
        }
    }
}
