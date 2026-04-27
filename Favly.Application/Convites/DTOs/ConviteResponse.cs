using Favly.Domain.Common.Enums;
using Favly.Domain.Entities;

namespace Favly.Application.Convites.DTOs
{
    public record ConviteResponse(
        Guid Id,
        string EmailConvidado,
        string Codigo,
        string Status,
        DateTime DataExpiracao,
        DateTime DataCriacao)
    {
        public static ConviteResponse FromEntity(Convite c) => new(
            c.Id,
            c.EmailConvidado.EnderecoEmail,
            c.Codigo,
            c.Status.ToString(),
            c.DataExpiracao,
            c.DataCriacao);
    }
}
