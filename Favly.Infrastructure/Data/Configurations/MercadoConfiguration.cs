using Favly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Infrastructure.Data.Configurations
{
    public class MercadoConfiguration : IEntityTypeConfiguration<Mercado>
    {
        public void Configure(EntityTypeBuilder<Mercado> builder)
        {
            builder.ToTable("Mercados");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Nome).HasMaxLength(100).IsRequired();
            builder.Property(m => m.Endereco).HasMaxLength(255);
            builder.Property(m => m.GrupoId).IsRequired();
        }
    }
}
