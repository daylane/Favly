using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string senha);
        bool Verificar(string senha, string hash);
    }
}
