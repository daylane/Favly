namespace Favly.Application.Grupos.Commands.ExpulsarMembro
{
    public record ExpulsarMembroCommand(Guid GrupoId, Guid AdminId, Guid MembroId);
}
