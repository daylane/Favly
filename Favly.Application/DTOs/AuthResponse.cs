using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.DTOs
{
    public record AuthResponse(bool Success, string Token, string Message = "");
}
