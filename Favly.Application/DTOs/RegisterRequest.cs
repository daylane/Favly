using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Favly.Application.DTOs
{
    public record RegisterRequest(

    [property: Required(ErrorMessage = "O e-mail é obrigatório")]
    [property: EmailAddress(ErrorMessage = "E-mail em formato inválido")]
    string Email,

    [property: Required(ErrorMessage = "A senha é obrigatória")]
    [property: StringLength(100, ErrorMessage = "A {0} deve ter no mínimo {2} caracteres", MinimumLength = 6)]
    string Password,

    [property: Compare("Password", ErrorMessage = "As senhas não conferem")]
    string ConfirmPassword);
}