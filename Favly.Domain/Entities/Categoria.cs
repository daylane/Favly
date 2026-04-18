using Favly.Domain.Common.Base;
using Favly.Domain.Common.Exceptions;
using Favly.Domain.Common.Validations;

namespace Favly.Domain.Entities
{
    public class Categoria : Entity
    {
        public Guid GrupoId { get; private set; }
        public string Nome { get; private set; }
        public string Icone { get; private set; } // ex: "🥩", "🥛", "🧴"

        protected Categoria() { }

        private Categoria(Guid grupoId, string nome, string icone)
        {
            GrupoId = grupoId;
            Nome = nome;
            Icone = icone;
        }

        public static Categoria Criar(Guid grupoId, string nome, string icone = "📦")
        {
            Guard.AgainstEmptyGuid(grupoId, nameof(grupoId));
            Guard.AgainstNullOrWhiteSpace(nome, nameof(nome));
            DomainException.When(nome.Length > 50, "Nome da categoria não pode ter mais de 50 caracteres.");

            return new Categoria(grupoId, nome.Trim(), icone);
        }

        public void Atualizar(string nome, string icone)
        {
            Guard.AgainstNullOrWhiteSpace(nome, nameof(nome));
            DomainException.When(nome.Length > 50, "Nome da categoria não pode ter mais de 50 caracteres.");

            Nome = nome.Trim();
            Icone = icone;
            AtualizarDataAtualizacao();
        }

        public void Desativar()
        {
            DomainException.When(!Ativo, "Categoria já está inativa.");
            AtualizarAtivo();
            AtualizarDataAtualizacao();
        }
    }
}