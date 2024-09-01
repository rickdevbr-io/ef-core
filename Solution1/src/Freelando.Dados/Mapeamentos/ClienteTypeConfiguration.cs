using Freelando.Modelo;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelando.Dados.Mapeamentos;
internal class ClienteTypeConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> entity)
    {
        entity.ToTable("TB_Clientes");
        entity.HasKey(e => e.Id).HasName("PK_TB_Clientes");
        entity.Property(e => e.Id).HasColumnName("Id_Cliente");
        entity.Property(e => e.Nome).HasColumnName("Nome");
        entity.Property(e => e.Cpf).HasColumnName("Cpf");
        entity.Property(e => e.Email).HasColumnName("Email");
        entity.Property(e => e.Telefone).HasColumnName("Telefone");
    }
}