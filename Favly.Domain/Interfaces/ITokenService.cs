using Favly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user);
    }
}
