using Favly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Infrastructure.Data.Configurations
{
    public class TarefaConfiguration : IEntityTypeConfiguration<Tarefa>
    {
        public void Configure(EntityTypeBuilder<Tarefa> builder)
        {
            builder.ToTable("Tarefas");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Titulo).HasMaxLength(150).IsRequired();

            // Mapeando o Value Object de Recorrência (DDD)
            builder.OwnsOne(t => t.Recorrencia, r =>
            {
                r.Property(x => x.Frequencia).HasColumnName("RecorrenciaTipo");
                r.Property(x => x.Intervalo).HasColumnName("RecorrenciaIntervalo");
            });

            // Configuração de Subtarefas (Auto-relacionamento)
            builder.HasOne<Tarefa>()
                   .WithMany()
                   .HasForeignKey(t => t.TarefaPaiId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
