using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Common.Base
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        public DateTime DataCriacao { get; protected set; }
        public DateTime DataAtualizacao { get; protected set; }
        public bool Ativo { get; protected set; }

        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent domainEvent)
            => _domainEvents.Add(domainEvent);
        public void RemoveDomainEvent(IDomainEvent domainEvent)
            => _domainEvents.Remove(domainEvent);
        public void ClearDomainEvents()
            => _domainEvents.Clear();

        protected Entity()
        {
            Id = Guid.NewGuid();
            DataCriacao = DateTime.UtcNow;
            Ativo = true;
        }
        protected void AtualizarDataAtualizacao()
            => DataAtualizacao = DateTime.UtcNow;
        protected void AtualizarAtivo()
            => Ativo = !Ativo;
        protected Entity(Guid id)
            => Id = id;
        public override bool Equals(object obj)
        {
            if (obj is not Entity other)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (GetType() != other.GetType())
                return false;
            return Id.Equals(other.Id);
        }
        public override int GetHashCode()
            =>  Id.GetHashCode();

        public static bool operator ==(Entity left, Entity right)
        {
            if (left is null)
                return right is null;
            return left.Equals(right);
        }
        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
    }
}
