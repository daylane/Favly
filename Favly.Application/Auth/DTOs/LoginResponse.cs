using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Auth.DTOs
{
    public record LoginResponse(
      string Token,
      string Nome,
      string Email,
      string? Avatar,
      Guid GrupoId,
      string GrupoNome,
      DateTime Expiracao);
}
