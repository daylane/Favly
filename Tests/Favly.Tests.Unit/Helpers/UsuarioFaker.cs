using Bogus;
using Favly.Application.Auth.Commands.Login;
using Favly.Application.Usuarios.Commands.AtivarUsuario;
using Favly.Application.Usuarios.Commands.AtualizarUsuario;
using Favly.Application.Usuarios.Commands.CriarUsuario;
using Favly.Application.Usuarios.Commands.DesativarUsuario;
using Favly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Tests.Helpers
{
    public static class UsuarioFaker
    {
        private static readonly Faker _faker = new("pt_BR");

        // ── Entidade ────────────────────────────────────────────────────────

        public static Usuario CriarValido() =>
            Usuario.Criar(
                email: _faker.Internet.Email(),
                nome: _faker.Name.FullName(),
                hash: _faker.Random.Hash(),
                avatar: _faker.Internet.Avatar());

        public static Usuario CriarAtivo()
        {
            var usuario = CriarValido();
            usuario.Ativar(usuario.CodigoAtivacao);
            return usuario;
        }

        public static Usuario CriarComEmail(string email) =>
            Usuario.Criar(
                email: email,
                nome: _faker.Name.FullName(),
                hash: _faker.Random.Hash(),
                avatar: _faker.Internet.Avatar());

        // ── Commands ────────────────────────────────────────────────────────

        public static CriarUsuarioCommand CriarUsuarioCommand() =>
            new(
                Nome: _faker.Name.FullName(),
                Email: _faker.Internet.Email().ToLower(),
                Senha: "Senha@" + _faker.Random.Number(100, 999),
                Avatar: _faker.Internet.Avatar());

        public static CriarUsuarioCommand CriarUsuarioCommandComEmail(string email) =>
            new(
                Nome: _faker.Name.FullName(),
                Email: email.ToLower(),
                Senha: "Senha@123",
                Avatar: _faker.Internet.Avatar());

        public static AtualizarUsuarioCommand AtualizarUsuarioCommand(Guid usuarioId) =>
          new( UsuarioId: usuarioId,
              Nome: _faker.Name.FullName(),
              Avatar: _faker.Internet.Avatar());

        public static AtivarUsuarioCommand AtivarUsuarioCommand(string email, string codigo) =>
            new(Email: email, CodigoAtivacao: codigo);
        public static DesativarUsuarioCommand DesativarUsuarioCommand(Guid usuarioId) =>
            new(UsuarioId: usuarioId);

        public static LoginCommand LoginCommandValido(string email, string senha = "Senha@123") =>
            new(email, senha);
    }
}
