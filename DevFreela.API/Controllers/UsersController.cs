﻿using DevFreela.API.Models;
using DevFreela.Application.Commands.CreateUser;
using DevFreela.Application.Commands.LoginUser;
using DevFreela.Application.InputModels;
using DevFreela.Application.Queries.GetUser;
//using DevFreela.Application.Services.Interfaces;
using DevFreela.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevFreela.API.Controllers
{
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        ////private readonly IUserService _userService; -- Instanciação da camada Service (Que pode ser usada, mas optei pelo CQRS)
        //public UsersController(IUserService userService)
        //{
        //    _userService = userService;
        //}
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // api/users/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            //Buscando via camada service
            //var user = _userService.GetUser(id);

            //if (user == null)
            //{
            //    return NotFound();
            //}

            //return Ok(user);

            //Buscando via CQRS
            var query = new GetUserQuery(id);

            var user = await _mediator.Send(query);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // api/users
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] CreateUserCommand command)
        {
            //Utilizando service
            //var id = _userService.Create(inputModel);
            //return CreatedAtAction(nameof(GetById), new { id = id }, inputModel);

            //CQRS
            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id = id }, command);
        }

        // api/users/login
        [HttpPut("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            // TODO: Para Módulo de Autenticação e Autorização
            var loginUserViewModel = await _mediator.Send(command); 
            if (loginUserViewModel == null) 
            {
                return BadRequest();
            }
            return Ok(loginUserViewModel);
        }
    }
}
