using DevFreela.Application.Queries.GetAllProjects;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DevFreela.Tests.Application.Queries
{
    public class GetAllProjectsCommandHandlerTests
    {
        [Fact]
        // Vou aplicar o padrão GWT (Given_When_Then) na nomenclatura do método. Given - ThreeProjectsExists When - Executed Then - ReturnThreeProjectsViewModel
        public async Task ThreeProjectsExists_Executed_ReturnThreeProjectsViewModel() 
        {
            //Aqui vou utilizar o padrão AAA (Arrange - Act - Assert) também.

            //Arrange
            var projects = new List<Project>
            {
               new Project("Projeto Teste 1","Descricao Teste 1",1,1,100),
               new Project("Projeto Teste 2","Descricao Teste 2",1,1,200),
               new Project("Projeto Teste 3","Descricao Teste 3",1,1,300),
            };

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(pr => pr.GetAllAsync().Result).Returns(projects);
            var getAllProjectsQuery = new GetAllProjectsQuery("");
            var getAllProjectsQueryHandler = new GetAllProjectsQueryHandler(projectRepositoryMock.Object);

            //Act
            var projectViewModelList = await getAllProjectsQueryHandler.Handle(getAllProjectsQuery, new CancellationToken());

            //Assert
            Assert.NotNull(projectViewModelList);
            Assert.NotEmpty(projectViewModelList);
            Assert.Equal(projects.Count,projectViewModelList.Count);

            projectRepositoryMock.Verify(pr => pr.GetAllAsync().Result, Times.Once);

        }
    }
}
