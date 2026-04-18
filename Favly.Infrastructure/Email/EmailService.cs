using Favly.Domain.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;


namespace Favly.Infrastructure.Email
{
    public class EmailService(IConfiguration _configuration, ILogger<EmailService> _logger) : IEmailService
    {
        public async Task EnviarCodigoAtivacaoAsync(string email, string nome, string codigo, CancellationToken ct = default)
        {
            var host = _configuration["Email:Host"] ?? throw new InvalidOperationException("Email:Host não configurado.");
            var port = int.Parse(_configuration["Email:Port"] ?? "587");
            var conta = _configuration["Email:Conta"] ?? throw new InvalidOperationException("Email:Conta não configurado.");
            var senha = _configuration["Email:Senha"] ?? throw new InvalidOperationException("Email:Senha não configurado.");
            var nomeRemetente = _configuration["Email:NomeRemetente"] ?? "Favly";

            var mensagem = new MimeMessage();
            mensagem.From.Add(new MailboxAddress(nomeRemetente, conta));
            mensagem.To.Add(new MailboxAddress(nome, email));
            mensagem.Subject = "Ative sua conta no Favly 🏠";
            mensagem.Body = new TextPart("html")
            {
                Text = GerarCorpoEmail(nome, codigo)
            };

            using var client = new SmtpClient();

            await client.ConnectAsync(host, port, SecureSocketOptions.Auto, ct);
            await client.AuthenticateAsync(conta, senha, ct);
            await client.SendAsync(mensagem, ct);
            await client.DisconnectAsync(true, ct);

            _logger.LogInformation("E-mail de ativação enviado para {Email}", email);
        }

        public async Task EnviarResetSenhaAsync(string email, string nome, string token, CancellationToken ct = default)
        {
            var host = _configuration["Email:Host"] ?? throw new InvalidOperationException("Email:Host não configurado.");
            var port = int.Parse(_configuration["Email:Port"] ?? "587");
            var conta = _configuration["Email:Conta"] ?? throw new InvalidOperationException("Email:Conta não configurado.");
            var senha = _configuration["Email:Senha"] ?? throw new InvalidOperationException("Email:Senha não configurado.");
            var nomeRemetente = _configuration["Email:NomeRemetente"] ?? "Favly";

            var mensagem = new MimeMessage();
            mensagem.From.Add(new MailboxAddress(nomeRemetente, conta));
            mensagem.To.Add(new MailboxAddress(nome, email));
            mensagem.Subject = "Redefinição de senha — Favly 🔐";
            mensagem.Body = new TextPart("html")
            {
                Text = GerarCorpoEmailResetSenha(nome, token)
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(host, port, SecureSocketOptions.StartTls, ct);
            await client.AuthenticateAsync(conta, senha, ct);
            await client.SendAsync(mensagem, ct);
            await client.DisconnectAsync(true, ct);

            _logger.LogInformation("E-mail de reset de senha enviado para {Email}", email);
        }

        private static string GerarCorpoEmailResetSenha(string nome, string token) => $"""
        <!DOCTYPE html>
        <html>
        <body style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;">
            <h2 style="color: #333;">Olá, {nome}! 👋</h2>
            <p>Recebemos uma solicitação para redefinir a senha da sua conta no <strong>Favly</strong>.</p>
            <p>Use o token abaixo para redefinir sua senha:</p>
            <div style="background-color: #f4f4f4; padding: 20px; border-radius: 8px; margin: 20px 0; word-break: break-all;">
                <code style="color: #4A90E2; font-size: 14px;">{token}</code>
            </div>
            <p style="color: #666; font-size: 14px;">
                Este token expira em <strong>1 hora</strong>.<br/>
                Se você não solicitou a redefinição de senha, ignore este e-mail.
            </p>
            <hr style="border: none; border-top: 1px solid #eee; margin: 20px 0;" />
            <p style="color: #999; font-size: 12px;">Favly — Organização para grupos e indivíduos</p>
        </body>
        </html>
        """;

        private static string GerarCorpoEmail(string nome, string codigo) => $"""
            <!DOCTYPE html>
            <html>
            <body style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;">
                <h2 style="color: #333;">Olá, {nome}! 👋</h2>
                <p>Obrigada por se cadastrar no <strong>Favly</strong>.</p>
                <p>Use o código abaixo para ativar sua conta:</p>
                <div style="background-color: #f4f4f4; padding: 20px; text-align: center; border-radius: 8px; margin: 20px 0;">
                    <h1 style="color: #4A90E2; letter-spacing: 8px; font-size: 36px; margin: 0;">
                        {codigo}
                    </h1>
                </div>
                <p style="color: #666; font-size: 14px;">
                    Este código expira em <strong>24 horas</strong>.<br/>
                    Se você não criou uma conta no Favly, ignore este e-mail.
                </p>
                <hr style="border: none; border-top: 1px solid #eee; margin: 20px 0;" />
                <p style="color: #999; font-size: 12px;">Favly — Organização para grupos e indivíduos</p>
            </body>
            </html>
            """;

        public Task EnviarAlertaEstoqueBaixoAsync(string email, string nome, string nomeProduto, decimal quantidadeAtual, decimal quantidadeMinima, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}

