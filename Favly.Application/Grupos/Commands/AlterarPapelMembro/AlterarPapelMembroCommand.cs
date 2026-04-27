using Favly.Domain.Common.Enums;

namespace Favly.Application.Grupos.Commands.AlterarPapelMembro
{
    public record AlterarPapelMembroCommand(Guid GrupoId, Guid AdminId, Guid MembroId, PapelMembro NovoPapel);
}
