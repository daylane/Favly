using Favly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
        Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken ct = default);
        Task<bool> EmailExisteAsync(string email, CancellationToken ct = default);
        Task AdicionarAsync(Usuario usuario, CancellationToken ct = default);
        void Atualizar(Usuario usuario);
    }
}
