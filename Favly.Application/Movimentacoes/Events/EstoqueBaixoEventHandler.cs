using Favly.Application.Abstractions.Persistence;
using Favly.Domain.Events.Estoque;
using Favly.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Movimentacoes.Events
{
    public class EstoqueBaixoEventHandler
    {
        public static async Task Handle(
            EstoqueBaixoEvent evento,
            IGrupoRepository grupoRepository,
            IEmailService emailService,
            CancellationToken ct)
        {
            var membros = await grupoRepository.ObterEmailsDoGrupoAsync(evento.GrupoId, ct);

            foreach (var (email, nome) in membros)
            {
                await emailService.EnviarAlertaEstoqueBaixoAsync(
                    email: email,
                    nome: nome,
                    nomeProduto: evento.NomeProduto,
                    quantidadeAtual: evento.QuantidadeAtual,
                    quantidadeMinima: evento.QuantidadeMinima,
                    ct: ct);
            }
        }
    }
}