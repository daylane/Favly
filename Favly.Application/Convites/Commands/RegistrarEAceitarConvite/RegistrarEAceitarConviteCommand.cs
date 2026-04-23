namespace Favly.Application.Convites.Commands.RegistrarEAceitarConvite
{
    public record RegistrarEAceitarConviteCommand(
        string Codigo,
        string Nome,
        string Senha,
        string Apelido,
        string? Avatar);
}
