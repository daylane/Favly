using Favly.Domain.Common.Base;
using Favly.Domain.Common.Enums;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Common.Validations;
using Favly.Domain.Events.Estoque;

namespace Favly.Domain.Entities
{
    public class Produto : Entity
    {
        public Guid GrupoId { get; private set; }
        public Guid CategoriaId { get; private set; }
        public string Nome { get; private set; }
        public string? Marca { get; private set; }
        public UnidadeMedida Unidade { get; private set; }
        public decimal QuantidadeAtual { get; private set; }
        public decimal QuantidadeMinima { get; private set; }

        // Histórico do melhor preço
        public decimal? UltimoPreco { get; private set; }
        public Guid? UltimoMercadoId { get; private set; }
        public DateTime? UltimaCompra { get; private set; }

        protected Produto() { }

        private Produto(
            Guid grupoId,
            Guid categoriaId,
            string nome,
            string? marca,
            UnidadeMedida unidade,
            decimal quantidadeMinima)
        {
            GrupoId = grupoId;
            CategoriaId = categoriaId;
            Nome = nome;
            Marca = marca;
            Unidade = unidade;
            QuantidadeAtual = 0;
            QuantidadeMinima = quantidadeMinima;
        }

        public static Produto Criar(
            Guid grupoId,
            Guid categoriaId,
            string nome,
            UnidadeMedida unidade,
            decimal quantidadeMinima,
            string? marca = null)
        {
            Guard.AgainstEmptyGuid(grupoId, nameof(grupoId));
            Guard.AgainstEmptyGuid(categoriaId, nameof(categoriaId));
            Guard.AgainstNullOrWhiteSpace(nome, nameof(nome));
            DomainException.When(nome.Length > 100, "Nome do produto não pode ter mais de 100 caracteres.");
            DomainException.When(quantidadeMinima < 0, "Quantidade mínima não pode ser negativa.");

            return new Produto(grupoId, categoriaId, nome.Trim(), marca?.Trim(), unidade, quantidadeMinima);
        }

        public void Atualizar(string nome, string? marca, Guid categoriaId, decimal quantidadeMinima)
        {
            Guard.AgainstNullOrWhiteSpace(nome, nameof(nome));
            Guard.AgainstEmptyGuid(categoriaId, nameof(categoriaId));
            DomainException.When(quantidadeMinima < 0, "Quantidade mínima não pode ser negativa.");

            Nome = nome.Trim();
            Marca = marca?.Trim();
            CategoriaId = categoriaId;
            QuantidadeMinima = quantidadeMinima;
            AtualizarDataAtualizacao();
        }

        public void AdicionarEstoque(decimal quantidade, decimal? preco, Guid? mercadoId)
        {
            DomainException.When(quantidade <= 0, "Quantidade deve ser maior que zero.");
            DomainException.When(
                !Unidade.PermiteDecimal() && quantidade % 1 != 0,
                $"O produto '{Nome}' usa a unidade '{Unidade.Sigla()}' que não aceita quantidade decimal. Informe um número inteiro.");

            QuantidadeAtual += quantidade;

            if (preco.HasValue)
            {
                UltimoPreco = preco;
                UltimoMercadoId = mercadoId;
                UltimaCompra = DateTime.UtcNow;
            }

            AtualizarDataAtualizacao();
        }

        public void RemoverEstoque(decimal quantidade)
        {
            DomainException.When(quantidade <= 0, "Quantidade deve ser maior que zero.");
            DomainException.When(
                !Unidade.PermiteDecimal() && quantidade % 1 != 0,
                $"O produto '{Nome}' usa a unidade '{Unidade.Sigla()}' que não aceita quantidade decimal. Informe um número inteiro.");
            DomainException.When(quantidade > QuantidadeAtual, "Quantidade insuficiente em estoque.");

            QuantidadeAtual -= quantidade;
            AtualizarDataAtualizacao();

            if (QuantidadeAtual <= QuantidadeMinima)
                AddDomainEvent(new EstoqueBaixoEvent(Id, GrupoId, Nome, QuantidadeAtual, QuantidadeMinima));
        }

        public void Desativar()
        {
            DomainException.When(!Ativo, "Produto já está inativo.");
            AtualizarAtivo();
            AtualizarDataAtualizacao();
        }

        public bool EstoqueAbaixoDoMinimo => QuantidadeAtual <= QuantidadeMinima;
    }
}