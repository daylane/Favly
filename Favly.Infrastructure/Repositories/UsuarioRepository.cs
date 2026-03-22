using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using Favly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Infrastructure.Repositories
{
    public class UsuarioRepository(FavlyDbContext _context) : IUsuarioRepository
    {

        public async Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken ct = default) =>
            await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id, ct);

        public async Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken ct = default) =>
            await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email.EnderecoEmail == email.ToLower(), ct);

        public async Task<bool> EmailExisteAsync(string email, CancellationToken ct = default) =>
            await _context.Usuarios
                .AnyAsync(u => u.Email.EnderecoEmail == email.ToLower(), ct);

        public async Task AdicionarAsync(Usuario usuario, CancellationToken ct = default) =>
            await _context.Usuarios.AddAsync(usuario, ct);

        public void Atualizar(Usuario usuario) =>
            _context.Usuarios.Update(usuario);
    }
}
