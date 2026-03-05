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

            builder.Property(t => t.MembrosAtribuidosIds)
             .HasColumnName("MembrosAtribuidosIds")
             .HasField("_membrosAtribuidosIds") 
             .UsePropertyAccessMode(PropertyAccessMode.Field)
             .HasColumnType("uuid[]"); // Tipo nativo do Postgres

            builder.OwnsOne(t => t.Recorrencia, r =>
            {
                r.Property(x => x.Frequencia).HasColumnName("RecorrenciaTipo");
                r.Property(x => x.Intervalo).HasColumnName("RecorrenciaIntervalo");
            });

            builder.HasOne<Tarefa>()
                   .WithMany()
                   .HasForeignKey(t => t.TarefaPaiId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
