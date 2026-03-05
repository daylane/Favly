using Favly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Infrastructure.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");
            builder.HasKey(u => u.Id);

            // O SEGREDO: Mapeando o EmailUsuario como Owned Type (KISS/DDD)
            builder.OwnsOne(u => u.Email, e =>
            {
                e.Property(x => x.EnderecoEmail)
                 .HasColumnName("Email") // Nome da coluna no banco
                 .HasMaxLength(255)
                 .IsRequired();

                // Garante que não existam dois usuários com o mesmo e-mail
                e.HasIndex(x => x.EnderecoEmail).IsUnique();
            });

            builder.Property(u => u.Nome).IsRequired().HasMaxLength(100);
        }
    }
}
