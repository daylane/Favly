using Favly.Application.Auth.Commands.Login;
using Favly.Application.Auth.DTOs;
using Favly.Tests.Helpers;
using Favly.Tests.Integration.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace Favly.Tests.Integration.Integration
{
    public class AuthIntegrationTests : IClassFixture<FavlyWebFactory>
    {
        private readonly HttpClient _client;

        public AuthIntegrationTests(FavlyWebFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_ComCredenciaisValidas_DeveRetornar200ComToken()
        {
            var (email, senha) = await TestHelpers.CriarEAtivarUsuario(_client);

            var response = await _client.PostAsJsonAsync("/api/auth/login",
                UsuarioFaker.LoginCommandValido(email, senha));

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            result!.Token.Should().NotBeNullOrEmpty();
            result.Email.Should().Be(email);
            result.Expiracao.Should().BeAfter(DateTime.UtcNow);
        }

        [Fact]
        public async Task Login_ComSenhaErrada_DeveRetornar400()
        {
            var (email, _) = await TestHelpers.CriarEAtivarUsuario(_client);

            var response = await _client.PostAsJsonAsync("/api/auth/login",
                UsuarioFaker.LoginCommandValido(email, "SenhaErrada"));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_ComEmailNaoCadastrado_DeveRetornar400()
        {
            var response = await _client.PostAsJsonAsync("/api/auth/login",
                new LoginCommand("naocadastrado@email.com", "Senha@123"));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_ComContaNaoAtivada_DeveRetornar400()
        {
            var command = UsuarioFaker.CriarUsuarioCommand();
            await _client.PostAsJsonAsync("/api/usuarios", command); 

            var response = await _client.PostAsJsonAsync("/api/auth/login",
                UsuarioFaker.LoginCommandValido(command.Email, command.Senha));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
