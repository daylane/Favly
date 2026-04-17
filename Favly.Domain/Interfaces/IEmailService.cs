using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Interfaces
{
    public interface IEmailService
    {
        Task EnviarCodigoAtivacaoAsync(string email, string nome, string codigo, CancellationToken ct = default);
        Task EnviarResetSenhaAsync(string email, string nome, string token, CancellationToken ct = default);
    }
}
