using Favly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Infrastructure.Data.Configurations
{
    internal class ConviteConfiguration : IEntityTypeConfiguration<Convite>
    {
        public void Configure(EntityTypeBuilder<Convite> builder)
        {
            builder.ToTable("Convites");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Codigo).HasMaxLength(10).IsRequired();

            // O MAPEAMENTO QUE FALTA (KISS/DDD)
            builder.OwnsOne(c => c.EmailConvidado, e =>
            {
                e.Property(x => x.EnderecoEmail)
                 .HasColumnName("EmailConvidado") // Nome da coluna na tabela Convites
                 .HasMaxLength(255)
                 .IsRequired();
            });

            builder.Property(c => c.Status).HasConversion<int>();
            builder.Property(c => c.DataExpiracao).IsRequired();
        }
    }
}
