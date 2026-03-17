using Favly.Domain.Common.Enums;
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

            builder.Property(t => t.Titulo)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(t => t.FamiliaId)
                .IsRequired(false); 

            builder.Property(t => t.MembroDonoId)
                .IsRequired();

            builder.Property(t => t.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(t => t.Escopo)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(t => t.MembrosAtribuidosIds)
                .HasColumnName("MembrosAtribuidosIds")
                .HasField("_membrosAtribuidosIds")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnType("uuid[]");

            builder.OwnsOne(t => t.Recorrencia, r =>
            {
                r.Property(x => x.Frequencia)       // enum FrequenciaTarefa
                    .HasColumnName("RecorrenciaTipo")
                    .HasConversion<int>()
                    .IsRequired();

                r.Property(x => x.Intervalo)
                    .HasColumnName("RecorrenciaIntervalo")
                    .IsRequired();

                r.Property(x => x.DiasDaSemana)
                    .HasColumnName("Recorrencia_DiasDaSemana")
                    .HasConversion(
                        v => v.Select(d => (int)d).ToList(),
                        v => v.Select(d => (DiasDaSemana)d).ToList())
                    .HasColumnType("integer[]");
            });

            // Auto-referência: subtarefas
            builder.HasOne<Tarefa>()
                .WithMany()
                .HasForeignKey(t => t.TarefaPaiId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
