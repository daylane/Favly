namespace Favly.Application.Convites.DTOs
{
    /// <summary>
    /// Usado por usuários que ainda não têm conta.
    /// O e-mail é inferido do convite; não é necessário enviá-lo no corpo.
    /// </summary>
    public record RegistrarEAceitarRequest(string Nome, string Senha, string Apelido, string? Avatar);
}
