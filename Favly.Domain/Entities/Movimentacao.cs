using Favly.Domain.Common.Base;
using Favly.Domain.Common.Enums;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Common.Validations;

namespace Favly.Domain.Entities
{
    public class Movimentacao : Entity
    {
        public Guid GrupoId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public Guid MembroId { get; private set; }
        public Guid? MercadoId { get; private set; }       // só em entradas
        public TipoMovimentacao Tipo { get; private set; }
        public decimal Quantidade { get; private set; }
        public decimal? PrecoUnitario { get; private set; }
        public decimal? ValorTotal { get; private set; }   // Quantidade × PrecoUnitario — persistido no momento do registro
        public string? Observacao { get; private set; }

        protected Movimentacao() { }

        private Movimentacao(
            Guid grupoId,
            Guid produtoId,
            Guid membroId,
            Guid? mercadoId,
            TipoMovimentacao tipo,
            decimal quantidade,
            decimal? precoUnitario,
            string? observacao)
        {
            GrupoId = grupoId;
            ProdutoId = produtoId;
            MembroId = membroId;
            MercadoId = mercadoId;
            Tipo = tipo;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
            ValorTotal = precoUnitario.HasValue ? quantidade * precoUnitario.Value : null;
            Observacao = observacao;
        }

        public static Movimentacao CriarEntrada(
            Guid grupoId,
            Guid produtoId,
            Guid membroId,
            decimal quantidade,
            decimal? preco = null,
            Guid? mercadoId = null,
            string? observacao = null)
        {
            Guard.AgainstEmptyGuid(grupoId, nameof(grupoId));
            Guard.AgainstEmptyGuid(produtoId, nameof(produtoId));
            Guard.AgainstEmptyGuid(membroId, nameof(membroId));
            DomainException.When(quantidade <= 0, "Quantidade deve ser maior que zero.");
            DomainException.When(preco.HasValue && preco < 0, "Preço não pode ser negativo.");

            return new Movimentacao(grupoId, produtoId, membroId, mercadoId,
                TipoMovimentacao.Entrada, quantidade, preco, observacao);
        }

        public static Movimentacao CriarSaida(
            Guid grupoId,
            Guid produtoId,
            Guid membroId,
            decimal quantidade,
            string? observacao = null)
        {
            Guard.AgainstEmptyGuid(grupoId, nameof(grupoId));
            Guard.AgainstEmptyGuid(produtoId, nameof(produtoId));
            Guard.AgainstEmptyGuid(membroId, nameof(membroId));
            DomainException.When(quantidade <= 0, "Quantidade deve ser maior que zero.");

            return new Movimentacao(grupoId, produtoId, membroId, null,
                TipoMovimentacao.Saida, quantidade, null, observacao);
        }
    }
}