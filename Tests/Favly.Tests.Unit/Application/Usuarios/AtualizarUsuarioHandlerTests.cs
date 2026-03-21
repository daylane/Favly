using Favly.Application.Usuarios.Commands.AtualizarUsuario;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using Favly.Tests.Helpers;
using FluentAssertions;
using NSubstitute;


namespace Favly.Tests.Unit.Application.Usuarios
{
    public class AtualizarUsuarioHandlerTests
    {
        private readonly IUsuarioRepository _repository = Substitute.For<IUsuarioRepository>();

        [Fact]
        public async Task Handle_ComDadosValidos_DeveAtualizarERetornarUsuario()
        {
            var usuario = UsuarioFaker.CriarAtivo();
            var command = UsuarioFaker.AtualizarUsuarioCommand(usuario.Id);

            _repository.ObterPorIdAsync(command.UsuarioId, default).Returns(usuario);

            var result = await AtualizarUsuarioHandler.Handle(command, _repository, default);

            result.Should().NotBeNull();
            result.Nome.Should().Be(command.Nome);
            result.Avatar.Should().Be(command.Avatar);
            result.Ativo.Should().BeTrue();

            _repository.Received(1).Atualizar(
                Arg.Is<Usuario>(u => u.Id == command.UsuarioId));
        }

        [Fact]
        public async Task Handle_ComGuidInvalido_DeveLancarNotFoundException()
        {
            var command = UsuarioFaker.AtualizarUsuarioCommand(Guid.NewGuid());

            _repository.ObterPorIdAsync(command.UsuarioId, default)
                .Returns((Usuario?)null);

            var act = async () => await AtualizarUsuarioHandler.Handle(command, _repository, default);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("*não encontrado*");
        }
    }
}
