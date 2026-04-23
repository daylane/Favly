namespace Favly.Domain.Interfaces
{
    public interface IEmailService
    {
        Task EnviarCodigoAtivacaoAsync(string email, string nome, string codigo, CancellationToken ct = default);
        Task EnviarResetSenhaAsync(string email, string nome, string token, CancellationToken ct = default);
        Task EnviarAlertaEstoqueBaixoAsync(string email, string nome, string nomeProduto,
            decimal quantidadeAtual, decimal quantidadeMinima, CancellationToken ct = default);
        Task EnviarConviteAsync(string email, string grupoNome, string codigo, CancellationToken ct = default);
    }
}
