using Favly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Infrastructure.Data.Configurations
{
    public class TokenResetSenhaConfiguration : IEntityTypeConfiguration<TokenResetSenha>
    {
        public void Configure(EntityTypeBuilder<TokenResetSenha> builder)
        {
            builder.ToTable("TokensResetSenha");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Token)
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(t => t.UsuarioId)
                .IsRequired();

            builder.Property(t => t.Expiracao)
                .IsRequired();

            builder.Property(t => t.Usado)
                .IsRequired();

            builder.HasIndex(t => t.Token).IsUnique();
        }
    }
}
