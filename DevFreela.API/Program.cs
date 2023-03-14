using DevFreela.API.Filters;
using DevFreela.API.Models;
using DevFreela.Application.Commands.CreateProject;
using DevFreela.Application.Commands.CreateUser;
using DevFreela.Application.Consumers;
//using DevFreela.Application.Services.Implementations;
//using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.Validators;
using DevFreela.Core.Repositories;
using DevFreela.Core.Services;
using DevFreela.Infrastructure.Auth;
using DevFreela.Infrastructure.MessageBus;
using DevFreela.Infrastructure.Payments;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Infrastructure.Persistence.Repositories;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.Configure<OpeningTimeOption>(builder.Configuration.GetSection("OpeningTime"));
//builder.Services.AddScoped<IProjectService, ProjectService>();

//Injeção de dependencia dos nossos serviços, repositorys
builder.Services.AddTransient<ISkillRepository, SkillRepository>();
builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddTransient<IMessageBusService, MessageBusService>();

//Adição do httpclient pro microsserviço de payments, caso necessário
builder.Services.AddHttpClient();

//Adição do hosted service (similar a um serviço que irá ficar coletando as informações da fila)
builder.Services.AddHostedService<PaymentApprovedConsumer>();


//Adição dos controllers com fluentValidation
builder.Services.AddControllers(options => options.Filters.Add(typeof(ValidationFilter)))
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUserCommandValidator>());

//Adição do MediatR pra implementação CQRS
builder.Services.AddMediatR(typeof(CreateProjectCommand));

//DbContext
builder.Services.AddDbContext<DevFreelaDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DevFreelaCs")));

//Configuração Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DevFreela.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Autenticação JWT utilizando scheme Bearer (Bearer Token)"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
                new string[]{ }
        }
    });
});

//Configuração do token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevFreela.API v1");
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
