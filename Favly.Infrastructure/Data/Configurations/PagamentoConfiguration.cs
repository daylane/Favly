using Favly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Infrastructure.Data.Configurations
{
    public class PagamentoConfiguration : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.ToTable("Pagamentos");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Titulo)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(p => p.FamiliaId)
                .IsRequired(false); // nullable: despesa individual não tem grupo

            builder.Property(p => p.TarefaId)
                .IsRequired(false); // nullable: despesa não exige tarefa vinculada

            builder.Property(p => p.DataPagamento)
                .IsRequired(false); // nullable: só preenchido ao pagar

            builder.Property(p => p.Escopo)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(p => p.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.OwnsOne(p => p.Valor, v =>
            {
                v.Property(x => x.Valor)
                    .HasColumnName("Montante")
                    .HasPrecision(18, 2)
                    .IsRequired();

                v.Property(x => x.Moeda)
                    .HasColumnName("Moeda")
                    .HasMaxLength(3)
                    .HasDefaultValue("BRL");
            });

            builder.OwnsOne(p => p.Recorrencia, r =>
            {
                r.Property(x => x.DiaVencimento)   
                    .HasColumnName("RecorrenciaDiaVencimento")
                    .IsRequired();

                r.Property(x => x.Frequencia)       
                    .HasColumnName("RecorrenciaTipo")
                    .HasConversion<int>()
                    .IsRequired();
            });
        }
    }
}
