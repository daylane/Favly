using Favly.Application.Usuarios.Commands.ReenviarCodigoAtivacao;
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
    public class ReenviarCodigoAtivacaoHandlerTests
    {
        private readonly IUsuarioRepository _repository = Substitute.For<IUsuarioRepository>();
        private readonly IEmailService _emailService = Substitute.For<IEmailService>();
        private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();

        [Fact]
        public async Task Handle_ComEmailValido_DeveReenviarCodigo()
        {
            var usuario = UsuarioFaker.CriarValido();
            var command = new ReenviarCodigoAtivacaoCommand(usuario.Email.EnderecoEmail);

            _repository.ObterPorEmailAsync(command.Email, default).Returns(usuario);

            await ReenviarCodigoAtivacaoHandler.Handle(
                command, _repository, _emailService, _uow, default);

            await _emailService.Received(1).EnviarCodigoAtivacaoAsync(
                usuario.Email.EnderecoEmail,
                usuario.Nome,
                Arg.Any<string>(),
                default);

            await _uow.Received(1).CommitAsync(default);
        }

        [Fact]
        public async Task Handle_ComEmailNaoCadastrado_DeveLancarNotFoundException()
        {
            var command = new ReenviarCodigoAtivacaoCommand("naocadastrado@email.com");
            _repository.ObterPorEmailAsync(command.Email, default)
                .Returns((Usuario?)null);

            var act = async () => await ReenviarCodigoAtivacaoHandler.Handle(
                command, _repository, _emailService, _uow, default);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("*não encontrado*");
        }

        [Fact]
        public async Task Handle_ComContaJaAtiva_DeveLancarDomainException()
        {
            var usuario = UsuarioFaker.CriarAtivo();
            var command = new ReenviarCodigoAtivacaoCommand(usuario.Email.EnderecoEmail);

            _repository.ObterPorEmailAsync(command.Email, default).Returns(usuario);

            var act = async () => await ReenviarCodigoAtivacaoHandler.Handle(
                command, _repository, _emailService, _uow, default);

            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("*já está ativa*");
        }

        [Fact]
        public async Task Handle_ComContaJaAtiva_NaoDeveEnviarEmail()
        {
            var usuario = UsuarioFaker.CriarAtivo();
            var command = new ReenviarCodigoAtivacaoCommand(usuario.Email.EnderecoEmail);

            _repository.ObterPorEmailAsync(command.Email, default).Returns(usuario);

            try
            {
                await ReenviarCodigoAtivacaoHandler.Handle(
                    command, _repository, _emailService, _uow, default);
            }
            catch { }

            await _emailService.DidNotReceive().EnviarCodigoAtivacaoAsync(
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), default);
        }
    }
}
