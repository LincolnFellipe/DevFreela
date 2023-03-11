using DevFreela.Application.Commands.CreateUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DevFreela.Application.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(p => p.Email).EmailAddress().WithMessage("Email Inválido");
            RuleFor(p => p.Password).Must(ValidPassword).WithMessage("Senha deve conter no mínimo: 8 Caracteres, com 1 número, 1 letra maiúscula, 1 letra minúscula, e 1 caracter especial ");
            RuleFor(p => p.FullName).NotNull().NotEmpty().WithMessage("Nome é obrigatório");
        }
        public bool ValidPassword(string password) 
        {
            var regex = new Regex(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$");
            return regex.IsMatch(password);
        }
    }
}
