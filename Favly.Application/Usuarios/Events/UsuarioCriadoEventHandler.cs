using Favly.Domain.Events.Usuario;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Usuarios.Events
{
    public class UsuarioCriadoEventHandler
    {
        public static async Task Handle(
           UsuarioCriadoEvent evento,
           IUsuarioRepository repository,
           IEmailService emailService,
           CancellationToken ct)
        {
            var usuario = await repository.ObterPorIdAsync(evento.UsuarioId, ct);

            if (usuario is null)
            {
                return; 
            }

            await emailService.EnviarCodigoAtivacaoAsync(
                email: usuario.Email.EnderecoEmail,
                nome: usuario.Nome,
                codigo: usuario.CodigoAtivacao,
                ct: ct);
        }
    }
}
