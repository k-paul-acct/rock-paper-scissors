using RockPaperScissors.Api.Data.Repositories;
using RockPaperScissors.Api.Extensions;
using RockPaperScissors.Api.Services.GameService;
using RockPaperScissors.Api.Services.IdGenerator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IIdGenerator, IdGenerator>();
builder.Services.AddSingleton<GameRepository>();
builder.Services.AddSingleton<PlayerRepository>();
builder.Services.AddSingleton<GameService>();

builder.AddValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGameEndpoints();

app.Run();