using Favly.Application.Usuarios.Commands.AtivarUsuario;
using Favly.Application.Usuarios.Commands.DesativarUsuario;
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
    public class DesativarUsuarioHandlerTests
    {
        private readonly IUsuarioRepository _repository = Substitute.For<IUsuarioRepository>();

        [Fact]
        public async Task Handle_ComDadosValidos_DeveDesativareERetornarUsuario()
        {
            var usuario = UsuarioFaker.CriarAtivo();
            var command = UsuarioFaker.DesativarUsuarioCommand(usuario.Id);

            _repository.ObterPorIdAsync(command.UsuarioId, default).Returns(usuario);

            var result = await DesativarUsuarioHandler.Handle(command, _repository, default);

            result.Should().BeTrue();

            _repository.Received(1).Atualizar(
                Arg.Is<Usuario>(u => u.Id == command.UsuarioId));
        }

        [Fact]
        public async Task Handle_ComGuidInvalido_DeveLancarNotFoundException()
        {
            var command = UsuarioFaker.DesativarUsuarioCommand(Guid.NewGuid());

            _repository.ObterPorIdAsync(command.UsuarioId, default).Returns((Usuario?)null);

            var act = async () => await DesativarUsuarioHandler.Handle(command, _repository, default);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("*não encontrado*");
        }
    }
}
