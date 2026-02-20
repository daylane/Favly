# 🏠 Projeto Favly: Camada de Domínio

Este documento descreve a implementação técnica da camada de domínio do sistema **Favly**, focada no controle familiar e organização de rotinas. A estrutura segue rigorosamente os conceitos de **DDD**, **DRY** e **Clean Architecture**.

---

## 🏗️ Estrutura de Identidade (Base)

Para garantir o princípio **DRY**, todas as entidades herdam da classe abstrata `Entity`, que gerencia a identidade única e auditoria básica.

### Implementação: `Entity.cs`
```csharp
public abstract class Entity
{
    public Guid Id { get; protected set; }
    public DateTime DataCriacao { get; protected set; }
    public DateTime DataAtualizacao { get; protected set; }
    public bool Ativo { get; protected set; }
}
```

---
## 🛡️ Camada de Proteção
Utilizamos Guard Clauses para impedir que objetos de domínio sejam instanciados em estado inválido (Fail-Fast).

### Implementação: `Guard.cs`
```csharp
public static void AgainstInvalidEnum<TEnum>(object value, string parameterName) where TEnum : Enum
{
    if (!Enum.IsDefined(typeof(TEnum), value))
        throw new DomainException($"Valor inválido para {parameterName}");
}
```

---

## 🗺️ Modelo de Dados

[Leitor de DBML](https://databasediagram.com/app)
```dbml
// --- Aggregate Root ---
Table Familia {
  Id guid [pk]
  Nome varchar(100) [not null]
  Convite varchar(10) [unique] // Value Object
  Ativo boolean [not null]
  DataCriacao datetime
  DataAtualizacao datetime
}

// --- Entities ---
Table Membro {
  Id guid [pk]
  FamiliaId guid [ref: > Familia.Id]
  UsuarioId guid [note: 'Link Identity']
  Nome varchar(100)
  Permissao int [note: '1-Admin, 2-Common']
  Ativo boolean [not null]
  DataCriacao datetime
  DataAtualizacao datetime
}

Table TarefaItem {
  Id guid [pk]
  FamiliaId guid [ref: > Familia.Id]
  MembroAttribuidoId guid [ref: > Membro.Id, null]
  Titulo varchar(150)
  Status int [note: '1-Pendente, 2-Completado']
  
  // Value Object: Recorrencia
  Recorencia_Tipo int 
  Recorencia_Intervalo int
  
  ProximaOcorrencia datetime

  Ativo boolean [not null]
  DataCriacao datetime
  DataAtualizacao datetime
}

Table NotificacaoPagamento {
  Id guid [pk]
  FamiliaId guid [ref: > Familia.Id]
  Titulo varchar(150)
  
  // Value Object: Dinheiro
  Valor decimal(18,2)
  Moeda varchar(3)
  
  DataVencimento datetime
  Pago boolean
}
```
---

## Boas Práticas

### Commits

* Tipo: Define o tipo de alteração (**feat** para nova funcionalidade, **fix** para correção de bug, **docs** para documentação, **chore** para tarefas diversas, **refactor** para refatoração). 

* Escopo: Indica a parte do código afetada (ex: frontend, auth, database).

* Descrição: Uma frase curta, no modo imperativo, começando com letra maiúscula e sem ponto final. 

> Ex: feat(auth): adicionar autenticação com Google. 