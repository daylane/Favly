using Favly.Application.Abstractions.Persistence;
using Favly.Application.Mercados.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;

namespace Favly.Application.Mercados.Commands.CriarMercado
{
    public class CriarMercadoHandler
    {
        public static async Task<MercadoResponse> Handle(
            CriarMercadoCommand command,
            IMercadoRepository repository,
            IGrupoRepository grupoRepository,
            IUnitOfWork uow,
            CancellationToken ct)
        {
            var ehMembro = await grupoRepository.UsuarioEhMembroAsync(command.GrupoId, command.UsuarioId, ct);
            AcessoNegadoException.When(!ehMembro, "Você não é membro deste grupo.");

            var mercado = Mercado.Criar(command.GrupoId, command.Nome, command.Endereco);

            await repository.AdicionarAsync(mercado, ct);
            await uow.CommitAsync(ct);

            return MercadoResponse.FromEntity(mercado);
        }
    }
}
