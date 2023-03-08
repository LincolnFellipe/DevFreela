using DevFreela.API.Models;
using DevFreela.Application.Commands.CreateComment;
using DevFreela.Application.Commands.CreateProject;
using DevFreela.Application.Commands.DeleteProject;
using DevFreela.Application.Commands.FinishProject;
using DevFreela.Application.Commands.StartProject;
using DevFreela.Application.Commands.UpdateProject;
using DevFreela.Application.InputModels;
using DevFreela.Application.Queries.GetAllProjects;
using DevFreela.Application.Queries.GetProjectById;
//using DevFreela.Application.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DevFreela.API.Controllers
{
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly OpeningTimeOption _option;
        //private readonly IProjectService _projectService;
        private readonly IMediator _mediator;
        public ProjectsController( IMediator mediator)
        {
           //_projectService = projectService;
            _mediator = mediator;
        }
        // api/projects?query=net core
        [HttpGet]
        public async Task<IActionResult> Get(string query)
        {
            // Buscar todos ou filtrar via camada service
            //var projects = _projectService.GetAll(query);

            //Buscando via padrão CQRS
            var getAllProjectsquery = new GetAllProjectsQuery(query);
            var projects = await _mediator.Send(getAllProjectsquery);
            return Ok(projects);
        }

        // api/projects/2
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            //// Buscar o projeto via camada de servico
            //var projects = _projectService.GetById(id);
            //if (projects == null)
            //{
            //    return NotFound();
            //}
            //else
            //{
            //    return Ok();
            //}
            //Buscando via padrão CQRS
            var query = new GetProjectByIdQuery(id);

            var project = await _mediator.Send(query);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProjectCommand command)
        {
            //Cadastrando pelo padrão CQRS - Mediator
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = id }, command);
        }

        // api/projects/2
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateProjectCommand command)
        {
            if (command.Description.Length > 200)
            {
                return BadRequest();
            }

            // Atualizo o objeto
           await _mediator.Send(command);
           return NoContent();
        }

        // api/projects/3 DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {         
            // Remover - CQRS
            var command = new DeleteProjectCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }

        // api/projects/1/comments POST
        [HttpPost("{id}/comments")]
        public async Task<IActionResult> PostComment(int id, [FromBody] CreateCommentCommand command)
        {
            await _mediator.Send(command); 
            return NoContent();
        }

        // api/projects/1/start
        [HttpPut("{id}/start")]
        public async Task<IActionResult> Start(int id)
        {
            //Buscando via camada service
            //_projectService.Start(id);
            //return NoContent();

            //Buscando via CQRS
            var command = new StartProjectCommand(id);

            await _mediator.Send(command);

            return NoContent();
        }

        // api/projects/1/finish
        [HttpPut("{id}/finish")]
        public async Task<IActionResult> Finish(int id)
        {
            //Buscando via camada de servico
            //_projectService.Finish(id);
            //return NoContent();

            //Buscando via CQRS
            var command = new FinishProjectCommand(id);

            await _mediator.Send(command);

            return NoContent();
        }
    }
}
