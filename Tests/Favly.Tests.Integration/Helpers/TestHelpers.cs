using Favly.Application.Auth.DTOs;
using Favly.Application.Usuarios.DTOs;
using Favly.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Favly.Tests.Integration.Helpers
{
    public class TestHelpers
    {
        public static async Task<(string Email, string Senha)> CriarEAtivarUsuario(HttpClient client)
        {
            var command = UsuarioFaker.CriarUsuarioCommand();
            var criarResponse = await client.PostAsJsonAsync("/api/usuarios", command);
            var usuario = await criarResponse.Content.ReadFromJsonAsync<UsuarioResponse>();

            await client.PostAsJsonAsync(
                $"/api/usuarios/{usuario!.Id}/ativar",
                new { Codigo = usuario.CodigoAtivacao });

            return (command.Email, command.Senha);
        }

        public static async Task<string> CriarAtivarELogar(HttpClient client)
        {
            var (email, senha) = await CriarEAtivarUsuario(client);

            var loginResponse = await client.PostAsJsonAsync("/api/auth/login",
                UsuarioFaker.LoginCommandValido(email, senha));

            var login = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
            return login!.Token;
        }
    }
}
