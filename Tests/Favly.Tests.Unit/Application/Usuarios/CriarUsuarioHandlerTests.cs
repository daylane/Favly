using Favly.Application.Usuarios.Commands.CriarUsuario;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using Favly.Tests.Helpers;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Tests.Unit.Application.Usuarios
{
    public class CriarUsuarioHandlerTests()
    {
        private readonly IUsuarioRepository _repository = Substitute.For<IUsuarioRepository>();
        private readonly IPasswordHasher _hasher = Substitute.For<IPasswordHasher>();

        [Fact]
        public async Task Handle_ComDadosValidos_DeveCriarERetornarUsuario()
        {
            var command = UsuarioFaker.CriarUsuarioCommand();
            _repository.EmailExisteAsync(command.Email, default).Returns(false);
            _hasher.Hash(command.Senha).Returns("hash-bcrypt");

            var result = await CriarUsuarioHandler.Handle(command, _repository, _hasher, default);

            result.Should().NotBeNull();
            result.Nome.Should().Be(command.Nome);
            result.Email.Should().Be(command.Email);
            result.Ativo.Should().BeFalse();

            await _repository.Received(1).AdicionarAsync(
                Arg.Is<Usuario>(u => u.Email.EnderecoEmail == command.Email),
                default);
        }

        [Fact]
        public async Task Handle_ComEmailJaExistente_DeveLancarDomainException()
        {
            var command = UsuarioFaker.CriarUsuarioCommand();
            _repository.EmailExisteAsync(command.Email, default).Returns(true);

            var act = async () => await CriarUsuarioHandler.Handle(command, _repository, _hasher, default);

            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("*e-mail*uso*");
        }

        [Fact]
        public async Task Handle_DeveHashearSenhaAntesDePassarParaEntidade()
        {
            var command = UsuarioFaker.CriarUsuarioCommand();
            _repository.EmailExisteAsync(command.Email, default).Returns(false);
            _hasher.Hash(command.Senha).Returns("hash-gerado");

            await CriarUsuarioHandler.Handle(command, _repository, _hasher, default);

            _hasher.Received(1).Hash(command.Senha);
            await _repository.Received(1).AdicionarAsync(
                Arg.Is<Usuario>(u => u.Hash == "hash-gerado"),
                default);
        }
    }
}
