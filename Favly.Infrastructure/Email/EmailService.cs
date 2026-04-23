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
        // ── helpers ──────────────────────────────────────────────────────────

        private (string host, int port, string conta, string senha, string remetente) LerConfigSmtp()
        {
            var host = _configuration["Email:Host"] ?? throw new InvalidOperationException("Email:Host não configurado.");
            var port = int.Parse(_configuration["Email:Port"] ?? "587");
            var conta = _configuration["Email:Conta"] ?? throw new InvalidOperationException("Email:Conta não configurado.");
            var senha = _configuration["Email:Senha"] ?? throw new InvalidOperationException("Email:Senha não configurado.");
            var remetente = _configuration["Email:NomeRemetente"] ?? "Favly";
            return (host, port, conta, senha, remetente);
        }

        private static async Task EnviarAsync(MimeMessage mensagem, string host, int port,
            string conta, string senha, CancellationToken ct)
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(host, port, SecureSocketOptions.Auto, ct);
            await client.AuthenticateAsync(conta, senha, ct);
            await client.SendAsync(mensagem, ct);
            await client.DisconnectAsync(true, ct);
        }

        private MimeMessage CriarMensagem(string nomeRemetente, string conta,
            string nomeDestinatario, string emailDestinatario, string assunto, string html)
        {
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress(nomeRemetente, conta));
            msg.To.Add(new MailboxAddress(nomeDestinatario, emailDestinatario));
            msg.Subject = assunto;
            msg.Body = new TextPart("html") { Text = html };
            return msg;
        }

        // ── métodos públicos ──────────────────────────────────────────────────

        public async Task EnviarCodigoAtivacaoAsync(string email, string nome, string codigo, CancellationToken ct = default)
        {
            var (host, port, conta, senha, remetente) = LerConfigSmtp();
            var msg = CriarMensagem(remetente, conta, nome, email,
                "Ative sua conta no Favly 🏠",
                CorpoAtivacao(nome, codigo));
            await EnviarAsync(msg, host, port, conta, senha, ct);
            _logger.LogInformation("E-mail de ativação enviado para {Email}", email);
        }

        public async Task EnviarResetSenhaAsync(string email, string nome, string token, CancellationToken ct = default)
        {
            var (host, port, conta, senha, remetente) = LerConfigSmtp();
            var msg = CriarMensagem(remetente, conta, nome, email,
                "Redefinição de senha — Favly 🔐",
                CorpoResetSenha(nome, token));
            await EnviarAsync(msg, host, port, conta, senha, ct);
            _logger.LogInformation("E-mail de reset de senha enviado para {Email}", email);
        }

        public async Task EnviarAlertaEstoqueBaixoAsync(string email, string nome, string nomeProduto,
            decimal quantidadeAtual, decimal quantidadeMinima, CancellationToken ct = default)
        {
            var (host, port, conta, senha, remetente) = LerConfigSmtp();
            var msg = CriarMensagem(remetente, conta, nome, email,
                $"⚠️ Estoque baixo: {nomeProduto}",
                CorpoEstoqueBaixo(nome, nomeProduto, quantidadeAtual, quantidadeMinima));
            await EnviarAsync(msg, host, port, conta, senha, ct);
            _logger.LogInformation("Alerta de estoque baixo enviado para {Email} — produto: {Produto}", email, nomeProduto);
        }

        public async Task EnviarConviteAsync(string email, string grupoNome, string codigo, CancellationToken ct = default)
        {
            var (host, port, conta, senha, remetente) = LerConfigSmtp();
            var frontendUrl = _configuration["App:FrontendUrl"] ?? "https://favly.app";
            var link = $"{frontendUrl}/convite/{codigo}";

            var msg = CriarMensagem(remetente, conta, email, email,
                $"Você foi convidado para o grupo \"{grupoNome}\" no Favly 🏠",
                CorpoConvite(grupoNome, codigo, link));
            await EnviarAsync(msg, host, port, conta, senha, ct);
            _logger.LogInformation("Convite enviado para {Email} — grupo: {Grupo}", email, grupoNome);
        }

        // ── templates HTML ────────────────────────────────────────────────────

        private static string CorpoAtivacao(string nome, string codigo) => $"""
            <!DOCTYPE html>
            <html>
            <body style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;">
                <h2 style="color: #333;">Olá, {nome}! 👋</h2>
                <p>Obrigado por se cadastrar no <strong>Favly</strong>.</p>
                <p>Use o código abaixo para ativar sua conta:</p>
                <div style="background-color: #f4f4f4; padding: 20px; text-align: center; border-radius: 8px; margin: 20px 0;">
                    <h1 style="color: #4A90E2; letter-spacing: 8px; font-size: 36px; margin: 0;">{codigo}</h1>
                </div>
                <p style="color: #666; font-size: 14px;">
                    Este código expira em <strong>24 horas</strong>.<br/>
                    Se você não criou uma conta no Favly, ignore este e-mail.
                </p>
                <hr style="border: none; border-top: 1px solid #eee; margin: 20px 0;" />
                <p style="color: #999; font-size: 12px;">Favly — Controle de estoque familiar</p>
            </body>
            </html>
            """;

        private static string CorpoResetSenha(string nome, string token) => $"""
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
                <p style="color: #999; font-size: 12px;">Favly — Controle de estoque familiar</p>
            </body>
            </html>
            """;

        private static string CorpoEstoqueBaixo(string nome, string nomeProduto,
            decimal quantidadeAtual, decimal quantidadeMinima) => $"""
            <!DOCTYPE html>
            <html>
            <body style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;">
                <h2 style="color: #333;">Olá, {nome}! 👋</h2>
                <p>Um produto do seu grupo está com <strong>estoque baixo</strong>:</p>
                <div style="background-color: #fff3cd; border-left: 4px solid #ffc107; padding: 16px; border-radius: 4px; margin: 20px 0;">
                    <h3 style="margin: 0 0 8px 0; color: #856404;">{nomeProduto}</h3>
                    <p style="margin: 0; color: #856404;">
                        Quantidade atual: <strong>{quantidadeAtual}</strong> &nbsp;|&nbsp;
                        Mínimo configurado: <strong>{quantidadeMinima}</strong>
                    </p>
                </div>
                <p>Lembre-se de reabastecer para manter o controle do seu estoque em dia.</p>
                <hr style="border: none; border-top: 1px solid #eee; margin: 20px 0;" />
                <p style="color: #999; font-size: 12px;">Favly — Controle de estoque familiar</p>
            </body>
            </html>
            """;

        private static string CorpoConvite(string grupoNome, string codigo, string link) => $"""
            <!DOCTYPE html>
            <html>
            <body style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;">
                <h2 style="color: #333;">Você foi convidado! 🎉</h2>
                <p>Alguém te convidou para participar do grupo <strong>{grupoNome}</strong> no <strong>Favly</strong>.</p>

                <div style="text-align: center; margin: 32px 0;">
                    <a href="{link}"
                       style="background-color: #4A90E2; color: white; padding: 14px 32px; text-decoration: none;
                              border-radius: 8px; font-size: 16px; font-weight: bold; display: inline-block;">
                        Aceitar convite
                    </a>
                </div>

                <p style="color: #666; font-size: 14px;">Ou acesse o link diretamente:</p>
                <p style="word-break: break-all;">
                    <a href="{link}" style="color: #4A90E2;">{link}</a>
                </p>

                <div style="background-color: #f4f4f4; padding: 16px; border-radius: 8px; margin: 20px 0; text-align: center;">
                    <p style="margin: 0 0 8px 0; color: #666; font-size: 14px;">Código do convite:</p>
                    <code style="font-size: 22px; letter-spacing: 4px; color: #333; font-weight: bold;">{codigo}</code>
                </div>

                <p style="color: #666; font-size: 14px;">
                    Este convite expira em <strong>7 dias</strong>.<br/>
                    Se você não esperava este e-mail, pode ignorá-lo com segurança.
                </p>
                <hr style="border: none; border-top: 1px solid #eee; margin: 20px 0;" />
                <p style="color: #999; font-size: 12px;">Favly — Controle de estoque familiar</p>
            </body>
            </html>
            """;
    }
}
