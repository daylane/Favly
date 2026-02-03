using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
