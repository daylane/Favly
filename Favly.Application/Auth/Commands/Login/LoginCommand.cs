using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Senha);
}
