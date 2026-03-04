using Favly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Infrastructure.Data.Configurations
{
    public class GrupoConfiguration : IEntityTypeConfiguration<Grupo>
    {
        public void Configure(EntityTypeBuilder<Grupo> builder)
        {
            builder.ToTable("Grupos");
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Nome).HasMaxLength(100).IsRequired();
            builder.Property(g => g.Convite).HasMaxLength(10).IsFixedLength();

            builder.HasMany(g => g.Membros)
                   .WithOne()
                   .HasForeignKey(m => m.FamiliaId)
                   .OnDelete(DeleteBehavior.Cascade);
        }

        public class MembroConfiguration : IEntityTypeConfiguration<Membro>
        {
            public void Configure(EntityTypeBuilder<Membro> builder)
            {
                builder.ToTable("Membros");
                builder.HasKey(m => m.Id);

                builder.Property(m => m.Apelido).HasMaxLength(50).IsRequired();
                builder.Property(m => m.Role).HasConversion<int>(); 
            }
        }
    }
}
