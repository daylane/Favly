using Favly.Application.Abstractions.Persistence;
using Favly.Application.Convites.DTOs;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Interfaces;

namespace Favly.Application.Convites.Queries.ObterConvite
{
    public class ObterConvitePorCodigoHandler
    {
        public static async Task<ConvitePublicoResponse> Handle(
            ObterConvitePorCodigoQuery query,
            IConviteRepository conviteRepository,
            IGrupoRepository grupoRepository,
            IUsuarioRepository usuarioRepository,
            CancellationToken ct)
        {
            var convite = await conviteRepository.ObterPorCodigoAsync(query.Codigo, ct);
            NotFoundException.When(convite is null, "Convite não encontrado.");

            DomainException.When(
                convite!.DataExpiracao < DateTime.UtcNow,
                "Este convite expirou.");

            DomainException.When(
                convite.Status != Favly.Domain.Common.Enums.StatusConvite.Pendente,
                "Este convite já foi utilizado.");

            var grupo = await grupoRepository.ObterPorIdAsync(convite.FamiliaId, ct);
            NotFoundException.When(grupo is null, "Grupo não encontrado.");

            var usuarioJaCadastrado = await usuarioRepository.EmailExisteAsync(
                convite.EmailConvidado.EnderecoEmail, ct);

            return new ConvitePublicoResponse(
                convite.Id,
                grupo!.Id,
                grupo.Nome,
                grupo.Avatar,
                convite.EmailConvidado.EnderecoEmail,
                convite.DataExpiracao,
                usuarioJaCadastrado);
        }
    }
}
