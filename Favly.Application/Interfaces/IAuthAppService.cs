using Favly.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Interfaces
{
    public interface IAuthAppService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
    }
}
