using IntMed.API.Configure;
using IntMed.Application.Commands.Agendas.Request;
using IntMed.Application.Commands.Consultas.Requests;
using IntMed.Application.Commands.Consultas.Response;
using IntMed.Application.Commands.Medicos;
using IntMed.Application.DTOs;
using IntMed.Application.Interfaces;
using IntMed.Application.Queries;
using IntMed.Infrasctructure.Factory;
using IntMed.Infrastructure.Factory;
using IntMed.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//Carregando key
var key = Encoding.ASCII.GetBytes(builder.Configuration["TokenConfigurations:secureKey"]);
//Recuperando connectionString
var connectionString = builder.Configuration.GetConnectionString("PostGreesConnection");
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region Configurando swagger e tokenização
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Token Authorization using Bearer Scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {new OpenApiSecurityScheme{
            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        }, new string [] { } }
    });
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.SaveToken = true;
    x.RequireHttpsMetadata = false;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
});

#endregion

#region Configura Services
builder.Services.AddTransient<IDataBaseConnectionFactory, DataBaseConnectionFactory>(x => new DataBaseConnectionFactory(connectionString));
builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();
builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();
builder.Services.AddScoped<IAgendaRepository, AgendaRepository>();

builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(CreateConsultaRequest).Assembly));

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "IntmedAPI v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

#region Rotas

# region Login

app.MapPost("/Login",  (string username, string password) =>
{
    if (username == null || password == null)
    {
        return Results.BadRequest("Login Inválido");
    }
    else
    {
        if (password == "IntMed" && username == "IntMed")
        {
            var token = new TokenFactory(builder).GenerateToken(username);
            return Results.Ok(new LoginResponseDTO
            {
                Autenticated = true,
                Message = "usuário autenticado com sucesso",
                AccessToken = token.AccessToken,
                Created = token.Created,
                Expiration = token.Expiration,
                username = username.ToString()
            });
        }
        else
            return Results.BadRequest(new LoginResponseDTO { Autenticated = false, Message = "Erro ao tentar autenticar" });
    }
}).AllowAnonymous();

#endregion

#region Consultas
app.MapPost("/consultas", async (IMediator mediator, CreateConsultaRequest par) =>
{
    var createdConsulta = await mediator.Send(par);
    if(createdConsulta == null)
    {
        return Results.NoContent();
    }
    else
    return Results.Created("Succes",createdConsulta); //aterar para created
});


app.MapGet("/consultas", async (IMediator mediator) =>
{
    var getcon = new GetAllConsultas();
    var result = await mediator.Send(getcon);
    if (result.Count() > 0)
    {
        return Results.Ok(result);
    }
    return Results.NoContent();
});

app.MapDelete("/consultas/{consulta_id}", async (IMediator mediator, int consulta_id) =>
{
    var deletedConsulta = new DeleteConsultaRequest { ConsultaId = consulta_id };

    var aux = mediator.Send(deletedConsulta);
    if (aux.IsCompletedSuccessfully)
    {
        return Results.Ok();
    }
    return Results.NoContent();

});

#endregion

#region Medicos

app.MapPost("/medicos", async (IMediator mediator, CreateMedicoRequest medicos) =>
{
    var createdMedico = await mediator.Send(medicos);
    if (createdMedico == null)
    {
        Results.NoContent();
    }

    return Results.Created("Succes", createdMedico);
});

#endregion

#region Agendas
app.MapGet("/agendas", async (IMediator mediator) =>
{
    var getcrm = new GetAllAgendas();
    var result = await mediator.Send(getcrm);

    return Results.Ok(result);
});


app.MapPost("/agendas", async (IMediator mediator, CreateAgendaRequest agenda) =>
{
    var createdAgenda = await mediator.Send(agenda);
    if (createdAgenda == null)
    {
        Results.NotFound();
    }

    return Results.Created("Succes", createdAgenda);
});
#endregion

#endregion

app.Run();