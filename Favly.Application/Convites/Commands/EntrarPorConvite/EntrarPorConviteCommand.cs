namespace Favly.Application.Convites.Commands.EntrarPorConvite
{
    /// <summary>
    /// Endpoint público. O e-mail vem do convite — o front não precisa enviá-lo.
    /// - Usuário existente: informe apenas Senha e Apelido.
    /// - Usuário novo:      informe Senha, Nome e Apelido (Avatar opcional).
    /// </summary>
    public record EntrarPorConviteCommand(
        string Codigo,
        string Senha,
        string? Nome,
        string Apelido,
        string? Avatar);
}
