using DevFreela.Application.ViewModels;
using DevFreela.Core.Repositories;
using DevFreela.Core.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Application.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserViewModel>
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        public LoginUserCommandHandler(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }
        public async Task<LoginUserViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            //Utilizar o mesmo método do createuser, para criar o hash dessa senha
            var passwordHash = _authService.ComputeSha256Hash(request.Password);

            //Buscar no banco de dados um user que tenha meu email e minha senha em formato hash
            var user = await _userRepository.GetUserByEmailAndPasswordAsync(request.Email, passwordHash);

            //Se nao existir = retorna o login nulo, para o controller tratar o erro
            if (user == null) 
            {
                return null;
            }
            //Se existir, gera o token com os dados do usuario
            var token = _authService.GenerateJwtToken(user.Email,user.Role);
            return new LoginUserViewModel(user.Email, token);
        }
    }
}
