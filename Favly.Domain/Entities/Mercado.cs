using Favly.Domain.Common.Base;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Common.Validations;

namespace Favly.Domain.Entities
{
    public class Mercado : Entity
    {
        public Guid GrupoId { get; private set; }
        public string Nome { get; private set; }
        public string? Endereco { get; private set; }

        protected Mercado() { }

        private Mercado(Guid grupoId, string nome, string? endereco)
        {
            GrupoId = grupoId;
            Nome = nome;
            Endereco = endereco;
        }

        public static Mercado Criar(Guid grupoId, string nome, string? endereco = null)
        {
            Guard.AgainstEmptyGuid(grupoId, nameof(grupoId));
            Guard.AgainstNullOrWhiteSpace(nome, nameof(nome));
            DomainException.When(nome.Length > 100, "Nome do mercado não pode ter mais de 100 caracteres.");

            return new Mercado(grupoId, nome.Trim(), endereco?.Trim());
        }

        public void Atualizar(string nome, string? endereco)
        {
            Guard.AgainstNullOrWhiteSpace(nome, nameof(nome));
            DomainException.When(nome.Length > 100, "Nome do mercado não pode ter mais de 100 caracteres.");

            Nome = nome.Trim();
            Endereco = endereco?.Trim();
            AtualizarDataAtualizacao();
        }

        public void Desativar()
        {
            DomainException.When(!Ativo, "Mercado já está inativo.");
            AtualizarAtivo();
            AtualizarDataAtualizacao();
        }
    }
}