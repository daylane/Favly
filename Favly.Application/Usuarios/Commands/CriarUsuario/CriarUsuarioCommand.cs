using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Usuarios.Commands.CriarUsuario
{
    public record CriarUsuarioCommand(
       string Nome,
       string Email,
       string Senha,
       string? Avatar = null);
}
