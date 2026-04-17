using Favly.Domain.Common.Exceptions;
using Favly.Domain.Entities;
using Favly.Tests.Helpers;
using FluentAssertions;

namespace Favly.Tests.Unit.Domain
{
    public class UsuarioTests
    {
        [Fact]
        public void Criar_ComDadosValidos_DeveCriarUsuario()
        {
            var usuario = UsuarioFaker.CriarValido();

            usuario.Should().NotBeNull();
            usuario.Nome.Should().NotBeNullOrEmpty();
            usuario.Email.EnderecoEmail.Should().Contain("@");
            usuario.Ativo.Should().BeFalse();
            usuario.CodigoAtivacao.Should().HaveLength(8);
        }

        [Fact]
        public void Criar_ComEmailInvalido_DeveLancarDomainException()
        {
            var act = () => UsuarioFaker.CriarComEmail("email-invalido");

            act.Should().Throw<DomainException>()
                .WithMessage("*formato*");
        }

        [Fact]
        public void Criar_ComNomeVazio_DeveLancarExcecao()
        {
            var act = () => Usuario.Criar("valido@email.com", "", "hash123", null);

            act.Should().Throw<Exception>();
        }

        [Fact]
        public void Criar_ComNomeMaiorQue100Caracteres_DeveLancarDomainException()
        {
            var nomeGrande = new string('A', 101);

            var act = () => Usuario.Criar("valido@email.com", nomeGrande, "hash123", null);

            act.Should().Throw<DomainException>()
                .WithMessage("*100*");
        }

        [Fact]
        public void Criar_DeveEmitirUsuarioCriadoEvent()
        {
            var usuario = UsuarioFaker.CriarValido();

            usuario.DomainEvents.Should().ContainSingle(e =>
                e.GetType().Name == "UsuarioCriadoEvent");
        }

        // ── Ativar ─────────────────────────────────────────────────────────

        [Fact]
        public void Ativar_ComCodigoCorreto_DeveAtivarUsuario()
        {
            var usuario = UsuarioFaker.CriarValido();
            var codigo = usuario.CodigoAtivacao;

            usuario.Ativar(codigo);

            usuario.Ativo.Should().BeTrue();
            usuario.CodigoAtivacao.Should().BeEmpty();
        }

        [Fact]
        public void Ativar_ComCodigoErrado_DeveLancarDomainException()
        {
            var usuario = UsuarioFaker.CriarValido();

            var act = () => usuario.Ativar("CODIGOERRADO");

            act.Should().Throw<DomainException>()
                .WithMessage("*inválido*");
        }

        [Fact]
        public void Ativar_UsuarioJaAtivo_DeveLancarDomainException()
        {
            var usuario = UsuarioFaker.CriarAtivo();

            var act = () => usuario.Ativar("QUALQUERCODIG");

            act.Should().Throw<DomainException>()
                .WithMessage("*já está ativo*");
        }

        [Fact]
        public void GerarCodigo_UsuarioInativo_DeveGerarNovoCodigo()
        {
            var usuario = UsuarioFaker.CriarValido();
            var codigoAntigo = usuario.CodigoAtivacao;

            usuario.GerarCodigo();

            usuario.CodigoAtivacao.Should().NotBe(codigoAntigo);
            usuario.CodigoAtivacao.Should().HaveLength(8);
        }

        [Fact]
        public void GerarCodigo_UsuarioAtivo_DeveLancarDomainException()
        {
            var usuario = UsuarioFaker.CriarAtivo();

            var act = () => usuario.GerarCodigo();

            act.Should().Throw<DomainException>()
                .WithMessage("*já está ativo*");
        }

        [Fact]
        public void GerarCodigo_DeveEmitirCodigoAtivacaoReenviadoEvent()
        {
            var usuario = UsuarioFaker.CriarValido();

            usuario.GerarCodigo();

            usuario.DomainEvents.Should().ContainSingle(e =>
                e.GetType().Name == "CodigoAtivacaoReenviadoEvent");
        }

        // ── Atualizar ──────────────────────────────────────────────────────

        [Fact]
        public void Atualizar_ComDadosValidos_DeveAtualizarUsuario()
        {
            var usuario = UsuarioFaker.CriarAtivo();

            usuario.Atualizar("Novo Nome", "novo-avatar.png");

            usuario.Nome.Should().Be("Novo Nome");
            usuario.Avatar.Should().Be("novo-avatar.png");
        }

        [Fact]
        public void Atualizar_UsuarioInativo_DeveLancarDomainException()
        {
            var usuario = UsuarioFaker.CriarValido();

            var act = () => usuario.Atualizar("Novo Nome", null);

            act.Should().Throw<DomainException>()
                .WithMessage("*inativo*");
        }

        // ── Desativar ──────────────────────────────────────────────────────

        [Fact]
        public void Desativar_UsuarioAtivo_DeveDesativarUsuario()
        {
            var usuario = UsuarioFaker.CriarAtivo();

            usuario.Desativar();

            usuario.Ativo.Should().BeFalse();
        }

        [Fact]
        public void Desativar_UsuarioJaInativo_DeveLancarDomainException()
        {
            var usuario = UsuarioFaker.CriarValido();

            var act = () => usuario.Desativar();

            act.Should().Throw<DomainException>()
                .WithMessage("*já está inativo*");
        }

        // ── ValidarSenha ───────────────────────────────────────────────────

        [Fact]
        public void ValidarSenha_ComHashCorreto_DeveRetornarTrue()
        {
            var usuario = UsuarioFaker.CriarAtivo();

            usuario.ValidarSenha(usuario.Hash).Should().BeTrue();
        }

        [Fact]
        public void ValidarSenha_ComHashErrado_DeveRetornarFalse()
        {
            var usuario = UsuarioFaker.CriarAtivo();

            usuario.ValidarSenha("hash-errado").Should().BeFalse();
        }
    }
}
