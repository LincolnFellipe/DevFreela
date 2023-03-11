using DevFreela.Core.Entities;
using DevFreela.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DevFreela.Tests.Core.Entities
{
    public class ProjectTests
    {
        [Fact]
        public void TestIfProjectStartsWorks() 
        {
            //Inicializo o objeto
            var project = new Project("Title","Desc",1,2,20);

            //Verifico se antes do start, o projeto está com o status Created
            Assert.Equal(ProjectStatusEnum.Created, project.Status);

            //Verifico se a data de inicio do projeto está nula, afinal de contas o projeto ainda não foi startado
            Assert.Null(project.StartedAt);

            //Verifico se o título não está vazio
            Assert.NotEmpty(project.Title);
            Assert.NotNull(project.Title);

            //Verifico se a descrição não está vazia
            Assert.NotEmpty(project.Description);
            Assert.NotNull(project.Description);

            //Chamo o método para dar start no projeto
            project.Start();

            //Verifico se o status foi alterado para InProgress
            Assert.Equal(ProjectStatusEnum.InProgress,project.Status);

            //Verifico se a data de inicio foi preenchida
            Assert.NotNull(project.StartedAt);
            
        }
    }
}
