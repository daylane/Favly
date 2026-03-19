using Bogus;
using Favly.Application.Auth.Commands.Login;
using Favly.Application.Usuarios.Commands.CriarUsuario;
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
                avatar: null);

        // ── Commands ────────────────────────────────────────────────────────

        public static CriarUsuarioCommand CriarUsuarioCommand() =>
            new(
                Nome: _faker.Name.FullName(),
                Email: _faker.Internet.Email().ToLower(),
                Senha: "Senha@" + _faker.Random.Number(100, 999),
                Avatar: null);

        public static CriarUsuarioCommand CriarUsuarioCommandComEmail(string email) =>
            new(
                Nome: _faker.Name.FullName(),
                Email: email.ToLower(),
                Senha: "Senha@123",
                Avatar: null);

        public static LoginCommand LoginCommandValido(string email, string senha = "Senha@123") =>
            new(email, senha);
    }
}
