using DevFreela.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Infrastructure.Persistence.Configurations
{
    public class ProjectConfigurations : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            //Configurando o EF - Project
            //Definindo a chave primária
            builder.HasKey(p => p.Id);
            //Adicionando os relacionamentos, 1-1, 1-n, n-n e por fim adicionando a chave estrangeira, o OnDelete vai servir pra impedir o drop de uma tabela com relacionamento.
            builder.HasOne(p => p.Freelancer).WithMany(f => f.FreelanceProjects).HasForeignKey(p => p.IdFreelancer).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Client).WithMany(f => f.OwnedProjects).HasForeignKey(p => p.IdClient).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
