using Favly.Application.DTOs;
using Favly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Mappers
{
    public static class UserMapper
    {
        public static ApplicationUser ToEntity(this RegisterRequest request)
        {
            return new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
            };
        }
    }
}
