using Favly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Interfaces
{
    public interface ITokenResetSenhaRepository
    {
        Task<TokenResetSenha?> ObterPorTokenAsync(string token, CancellationToken ct = default);
        Task AdicionarAsync(TokenResetSenha token, CancellationToken ct = default);
        void Atualizar(TokenResetSenha token);
    }
}
