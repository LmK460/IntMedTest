using IntMed.API.Configure;
using IntMed.Application.Commands.Consultas.Requests;
using IntMed.Application.Commands.Consultas.Response;
using IntMed.Application.Commands.Medicos;
using IntMed.Application.DTOs;
using IntMed.Application.Interfaces;
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
var connectionString = builder.Configuration.GetConnectionString("PostGreesConnection");
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

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


builder.Services.AddTransient<IDataBaseConnectionFactory, DataBaseConnectionFactory>(x => new DataBaseConnectionFactory(connectionString));
builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();
builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();

builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(CreateConsultaRequest).Assembly));

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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

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


app.MapPost("/consultas", async (IMediator mediator, CreateConsultaRequest par) =>
{
    var createdConsulta = await mediator.Send(par);
    return Results.CreatedAtRoute("GetById", new {createdConsulta.Id}, createdConsulta);
});


app.MapDelete("/consultas/{consulta_id}", async (IMediator mediator, int consulta_id) =>
{
    var deletedConsulta = new DeleteConsultaRequest { ConsultaId = consulta_id };

    var aux = mediator.Send(deletedConsulta);
    if (aux.IsCompletedSuccessfully)
    {
        return Results.Ok();
    }
    else return Results.NotFound();
}).AllowAnonymous();


app.MapPost("/medicos", async (IMediator mediator, CreateMedicoRequest par) =>
{
    var createdConsulta = await mediator.Send(par);
    if(createdConsulta == null)
    {
        Results.NotFound(createdConsulta);
    }

    return Results.Ok(createdConsulta);
}).AllowAnonymous();

app.MapGet("/crm/{id}", async (IMediator mediator, int crm) =>
{
    var getcrm = new CreateMedicoRequest{ CRM = crm};
    var aux = await mediator.Send(getcrm);

    return Results.Ok();
}).AllowAnonymous(); 





app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           summaries[Random.Shared.Next(summaries.Length)]
       ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}