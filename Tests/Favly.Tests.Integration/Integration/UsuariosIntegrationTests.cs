using Favly.Application.Usuarios.DTOs;
using Favly.Tests.Helpers;
using Favly.Tests.Integration.Helpers;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Favly.Tests.Integration.Integration
{
    public class UsuariosIntegrationTests : IClassFixture<FavlyWebFactory>
    {
        private readonly HttpClient _client;

        public UsuariosIntegrationTests(FavlyWebFactory factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task CriarUsuario_ComDadosValidos_DeveRetornar201()
        {
            var command = UsuarioFaker.CriarUsuarioCommand();

            var response = await _client.PostAsJsonAsync("/api/usuarios", command);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var result = await response.Content.ReadFromJsonAsync<UsuarioResponse>();
            result!.Nome.Should().Be(command.Nome);
            result.Email.Should().Be(command.Email);
            result.Ativo.Should().BeFalse();
        }
        [Fact]
        public async Task CriarUsuario_ComEmailDuplicado_DeveRetornar400()
        {
            var command = UsuarioFaker.CriarUsuarioCommandComEmail("duplicado@email.com");

            var primeiraResposta = await _client.PostAsJsonAsync("/api/usuarios", command);
            primeiraResposta.StatusCode.Should().Be(HttpStatusCode.Created);

            var response = await _client.PostAsJsonAsync("/api/usuarios", command);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CriarUsuario_ComEmailInvalido_DeveRetornar400()
        {
            var command = UsuarioFaker.CriarUsuarioCommandComEmail("email-invalido");

            var response = await _client.PostAsJsonAsync("/api/usuarios", command);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AtivarUsuario_ComCodigoCorreto_DeveRetornar204()
        {
            var command = UsuarioFaker.CriarUsuarioCommand();
            var criarResponse = await _client.PostAsJsonAsync("/api/usuarios", command);
            var usuario = await criarResponse.Content.ReadFromJsonAsync<UsuarioResponse>();

            var response = await _client.PostAsJsonAsync(
                $"/api/usuarios/{usuario!.Email}/ativar",
                new { Codigo = usuario.CodigoAtivacao });

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task AtivarUsuario_ComCodigoErrado_DeveRetornar400()
        {
            var command = UsuarioFaker.CriarUsuarioCommand();
            var criarResponse = await _client.PostAsJsonAsync("/api/usuarios", command);
            var usuario = await criarResponse.Content.ReadFromJsonAsync<UsuarioResponse>();

            var response = await _client.PostAsJsonAsync(
                $"/api/usuarios/{usuario!.Email}/ativar",
                new { Codigo = "ERRADO12" });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ObterUsuario_SemToken_DeveRetornar401()
        {
            _client.DefaultRequestHeaders.Authorization = null;

            var response = await _client.GetAsync($"/api/usuarios/{Guid.NewGuid()}");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ObterUsuario_ComIdInexistente_DeveRetornar404()
        {
            var token = await TestHelpers.CriarAtivarELogar(_client);
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"/api/usuarios/{Guid.NewGuid()}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}
