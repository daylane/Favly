using Favly.Application.Usuarios.Commands.AtivarUsuario;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using Favly.Tests.Helpers;
using FluentAssertions;
using NSubstitute;

namespace Favly.Tests.Unit.Application.Usuarios
{
    public class AtivarUsuarioHandlerTests()
    {
        private readonly IUsuarioRepository _repository = Substitute.For<IUsuarioRepository>();

        [Fact]
        public async Task Handle_ComDadosValidos_DeveAtivareERetornarUsuario()
        {
            var usuario = UsuarioFaker.CriarValido();
            var command = UsuarioFaker.AtivarUsuarioCommand(usuario.Id, usuario.CodigoAtivacao);

            _repository.ObterPorIdAsync(command.UsuarioId, default).Returns(usuario);

            var result = await AtivarUsuarioHandler.Handle(command, _repository, default);

            result.Should().BeTrue();

            _repository.Received(1).Atualizar(
                Arg.Is<Usuario>(u => u.Id == command.UsuarioId));
        }

        [Fact]
        public async Task Handle_ComGuidInvalido_DeveLancarNotFoundException()
        {
            var command = UsuarioFaker.AtivarUsuarioCommand(Guid.NewGuid(), "CODIGO-ERRADO");

            _repository.ObterPorIdAsync(command.UsuarioId, default).Returns((Usuario?)null);

            var act = async () => await AtivarUsuarioHandler.Handle(command, _repository, default);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("*não encontrado*");
        }
    }
}
