using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Auth.Commands.RedefinirSenha
{
    public record RedefinirSenhaCommand(string Token, string NovaSenha, string ConfirmacaoSenha);
}
