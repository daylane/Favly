using Favly.Application.Abstractions.Persistence;
using Favly.Application.Usuarios.DTOs;
using Favly.Domain.Common.Enums;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces;
using Wolverine;

namespace Favly.Application.Usuarios.Commands.CriarUsuario
{
    public class CriarUsuarioHandler
    {
        public static async Task<UsuarioResponse> Handle(
        CriarUsuarioCommand command,
        IUsuarioRepository repository,
        IPasswordHasher hasher,
        IUnitOfWork uow,
        IMessageBus bus,
        IGrupoRepository _grupoRepository,
        CancellationToken ct)
        {
            DomainException.When(
                await repository.EmailExisteAsync(command.Email, ct),
                "Este e-mail já está em uso.");

            var hash = hasher.Hash(command.Senha);
            var usuario = Usuario.Criar(command.Email, command.Nome, hash, command.Avatar);

            await repository.AdicionarAsync(usuario, ct);

            var grupoPessoal = new Grupo($"{command.Nome} - Pessoal");
            grupoPessoal.AdicionarMembro(usuario.Id, "Eu", PapelMembro.Administrador);

            await _grupoRepository.AdicionarAsync(grupoPessoal, ct);

            await uow.CommitAsync(ct);

            // Publica os domain events após salvar
            foreach (var evento in usuario.DomainEvents)
                await bus.PublishAsync(evento);

            usuario.ClearDomainEvents();

            return UsuarioResponse.FromEntity(usuario);
        }
    }
}
