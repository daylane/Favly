using Favly.Application.Auth.Commands.Login;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using Favly.Tests.Helpers;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Tests.Unit.Application.Auth
{
    public class LoginHandlerTests
    {
        private readonly IUsuarioRepository _repository = Substitute.For<IUsuarioRepository>();
        private readonly IPasswordHasher _hasher = Substitute.For<IPasswordHasher>();
        private readonly ITokenService _tokenService = Substitute.For<ITokenService>();


        [Fact]
        public async Task Handle_ComCredenciaisValidas_DeveRetornarToken()
        {
            var usuario = UsuarioFaker.CriarAtivo();
            var command = UsuarioFaker.LoginCommandValido(usuario.Email.EnderecoEmail);

            _repository.ObterPorEmailAsync(command.Email, default).Returns(usuario);
            _hasher.Verificar(command.Senha, usuario.Hash).Returns(true);
            _tokenService.GerarToken(usuario).Returns("token-jwt-gerado");

            var result = await LoginHandler.Handle(command, _repository, _hasher, _tokenService, default);

            result.Should().NotBeNull();
            result.Token.Should().Be("token-jwt-gerado");
            result.Email.Should().Be(usuario.Email.EnderecoEmail);
            result.Nome.Should().Be(usuario.Nome);
            result.Expiracao.Should().BeAfter(DateTime.UtcNow);
        }

        [Fact]
        public async Task Handle_ComEmailNaoCadastrado_DeveLancarDomainException()
        {
            var command = UsuarioFaker.LoginCommandValido("naocadastrado@email.com");
            _repository.ObterPorEmailAsync(command.Email, default).Returns((Usuario?)null);

            var act = async () => await LoginHandler.Handle(command, _repository, _hasher, _tokenService, default);

            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("*inválidos*");
        }

        [Fact]
        public async Task Handle_ComSenhaErrada_DeveLancarDomainException()
        {
            var usuario = UsuarioFaker.CriarAtivo();
            var command = UsuarioFaker.LoginCommandValido(usuario.Email.EnderecoEmail, "SenhaErrada");

            _repository.ObterPorEmailAsync(command.Email, default).Returns(usuario);
            _hasher.Verificar(command.Senha, usuario.Hash).Returns(false);

            var act = async () => await LoginHandler.Handle(command, _repository, _hasher, _tokenService, default);

            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("*inválidos*");
        }

        [Fact]
        public async Task Handle_ComContaNaoAtivada_DeveLancarDomainException()
        {
            var usuario = UsuarioFaker.CriarValido(); // não ativado
            var command = UsuarioFaker.LoginCommandValido(usuario.Email.EnderecoEmail);

            _repository.ObterPorEmailAsync(command.Email, default).Returns(usuario);

            var act = async () => await LoginHandler.Handle(command, _repository, _hasher, _tokenService, default);

            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("*não ativada*");
        }

        [Fact]
        public async Task Handle_ComCredenciaisValidas_DeveGerarTokenUmaVez()
        {
            var usuario = UsuarioFaker.CriarAtivo();
            var command = UsuarioFaker.LoginCommandValido(usuario.Email.EnderecoEmail);

            _repository.ObterPorEmailAsync(command.Email, default).Returns(usuario);
            _hasher.Verificar(command.Senha, usuario.Hash).Returns(true);
            _tokenService.GerarToken(usuario).Returns("token-jwt");

            await LoginHandler.Handle(command, _repository, _hasher, _tokenService, default);

            _tokenService.Received(1).GerarToken(usuario);
        }
    }
}
