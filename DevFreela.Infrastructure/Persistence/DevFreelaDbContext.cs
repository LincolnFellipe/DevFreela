using DevFreela.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Infrastructure.Persistence
{
    public class DevFreelaDbContext : DbContext
    {
        public DevFreelaDbContext(DbContextOptions<DevFreelaDbContext> options) : base(options)
        {
           
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }   
        public DbSet<Skill> Skills { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }   
        public DbSet<ProjectComment> ProjectComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            //Configurando o EF - Project
            //Definindo a chave primária
            modelBuilder.Entity<Project>().HasKey(p => p.Id);
            //Adicionando os relacionamentos, 1-1, 1-n, n-n e por fim adicionando a chave estrangeira, o OnDelete vai servir pra impedir o drop de uma tabela com relacionamento.
            modelBuilder.Entity<Project>().HasOne(p => p.Freelancer).WithMany(f => f.FreelanceProjects).HasForeignKey(p => p.IdFreelancer).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Project>().HasOne(p => p.Client).WithMany(f => f.OwnedProjects).HasForeignKey(p => p.IdClient).OnDelete(DeleteBehavior.Restrict);

            //Configurando o EF - ProjectComment
            modelBuilder.Entity<ProjectComment>().HasKey(p => p.Id);
            modelBuilder.Entity<ProjectComment>().HasOne(p => p.Project).WithMany(p => p.Comments).HasForeignKey(p => p.IdProject).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ProjectComment>().HasOne(p => p.User).WithMany(p => p.Comments).HasForeignKey(p => p.IdUser).OnDelete(DeleteBehavior.Restrict);
            //Configurando o EF - Skill
            modelBuilder.Entity<Skill>().HasKey(p => p.Id);

            //Configurando o EF - User
            modelBuilder.Entity<User>().HasKey(p => p.Id);
            modelBuilder.Entity<User>().HasMany(u => u.Skills).WithOne().HasForeignKey(u => u.IdSkill).OnDelete(DeleteBehavior.Restrict);

            //Configurando o EF - UserSkill
            modelBuilder.Entity<UserSkill>().HasKey(p => p.Id);
        }
    }
}
