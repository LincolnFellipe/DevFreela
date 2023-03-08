using DevFreela.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Infrastructure.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(string email, string role)
        {
            //Buscando as chaves dentro do appSettings
            var issuer = _configuration["Jwt:Issuer"];
            // O audience representa a aplicação que irá utilizar o token
            var audience = _configuration["Jwt:Audience"];
            var key = _configuration["Jwt:Key"];

            //Convertendo a chave key em bytes
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            //Assinatura do token com todos os dados
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Inicializando o papel (role) do usuário
            var claims = new List<Claim>
            {
                new Claim("userName", email),
                new Claim(ClaimTypes.Role,role)
            };

            //Inicializando o token, passando os parâmetros já definidos, e adicionando o tempo útil do token no expires
            var token = new JwtSecurityToken(issuer: issuer, audience: audience, expires: DateTime.Now.AddHours(8), signingCredentials: credentials, claims: claims);

            //Gerando a cadeia de caracteres
            var tokenHandler = new JwtSecurityTokenHandler();

            //Escrevendo a string do token e retornando
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }
        public string ComputeSha256Hash(string password)
        {
            //Inicializando o método do sha256 Create, que irá encriptar o password
            using (SHA256 sha256Hash = SHA256.Create())
            {
                //ComputeHash - retorna byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                //Converte byte array para string, usando o stringbuilder que concatena strings
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    // 2x faz com que sseja convertido em representação hexadecimal
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
