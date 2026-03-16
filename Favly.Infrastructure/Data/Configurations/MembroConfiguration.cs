using Favly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Infrastructure.Data.Configurations
{
    public class MembroConfiguration : IEntityTypeConfiguration<Membro>
    {
        public void Configure(EntityTypeBuilder<Membro> builder)
        {
            builder.ToTable("Membros");
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Apelido)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(m => m.Role)
                .HasConversion<int>()
                .IsRequired();
        }
    }
}
