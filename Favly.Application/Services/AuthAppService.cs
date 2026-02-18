using Favly.Application.DTOs;
using Favly.Application.Interfaces;
using Favly.Application.Mappers;
using Favly.Domain.Entities;
using Favly.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Services
{
    public class AuthAppService(UserManager<ApplicationUser> _userManager, ITokenService _tokenService) : IAuthAppService
    {
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return new AuthResponse(false, "", "Usuário ou senha inválidos.");

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
                return new AuthResponse(false, "", "Usuário ou senha inválidos.");

            var token = _tokenService.GenerateToken(user);

            return new AuthResponse(true, token, "Login realizado com sucesso!");
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var user = UserMapper.ToEntity(request);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
                return new AuthResponse(true, "", "Usuário criado com sucesso!");

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));

            return new AuthResponse(false, "", errors);
        }
    }
}
