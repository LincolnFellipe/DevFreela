using DevFreela.Application.Commands.CreateProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DevFreela.Tests.Application.Commands
{
    public class CreateProjectCommandHandlerTests
    {
        [Fact]
        public async Task InputDataIsOk_Executed_ProjectId() 
        {

            //Arrange
            var projectRepositoryMock = new Mock<IProjectRepository>();
            var createProjectCommand = new CreateProjectCommand() 
            { 
            Title = "Test",
            Description = "Test",
            TotalCost = 0,
            IdClient = 1,
            IdFreelancer = 1,
            };

            var creatProjectCommandHandler = new CreateProjectCommandHandler(projectRepositoryMock.Object);

            //Act
            var id = await creatProjectCommandHandler.Handle(createProjectCommand, new CancellationToken());


            //Assert
            Assert.True(id >= 0);
            projectRepositoryMock.Verify(pr => pr.AddAsync(It.IsAny<Project>()),Times.Once);
        
        }
    }
}
