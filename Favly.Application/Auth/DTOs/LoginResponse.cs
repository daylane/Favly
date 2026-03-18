using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Auth.DTOs
{
    public record LoginResponse(
      string Token,
      string Nome,
      string Email,
      DateTime Expiracao);
}
