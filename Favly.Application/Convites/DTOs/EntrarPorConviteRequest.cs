namespace Favly.Application.Convites.DTOs
{
    /// <summary>
    /// Body para POST /api/convites/{codigo}/entrar.
    /// Nome é obrigatório apenas quando o e-mail do convite não possui cadastro.
    /// </summary>
    public record EntrarPorConviteRequest(
        string Senha,
        string? Nome,
        string Apelido,
        string? Avatar);
}
