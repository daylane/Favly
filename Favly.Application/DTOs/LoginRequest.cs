using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.DTOs
{
    public record LoginRequest(string Email, string Password);
}
