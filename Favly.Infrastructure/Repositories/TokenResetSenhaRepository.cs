using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using Favly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Infrastructure.Repositories
{
    public class TokenResetSenhaRepository : ITokenResetSenhaRepository
    {
        private readonly FavlyDbContext _context;

        public TokenResetSenhaRepository(FavlyDbContext context)
            => _context = context;

        public async Task<TokenResetSenha?> ObterPorTokenAsync(string token, CancellationToken ct = default) =>
            await _context.TokensResetSenha
                .FirstOrDefaultAsync(t => t.Token == token, ct);

        public async Task AdicionarAsync(TokenResetSenha token, CancellationToken ct = default) =>
            await _context.TokensResetSenha.AddAsync(token, ct);

        public void Atualizar(TokenResetSenha token) =>
            _context.TokensResetSenha.Update(token);
    }
}
