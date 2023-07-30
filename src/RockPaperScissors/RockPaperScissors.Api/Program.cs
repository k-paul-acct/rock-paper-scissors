using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RockPaperScissors.Api.Data;
using RockPaperScissors.Api.Extensions;
using RockPaperScissors.Api.Services.GameScoreService;
using RockPaperScissors.Api.Services.GameService;
using RockPaperScissors.Api.Services.IdGenerator;
using RockPaperScissors.Api.Services.MoveService;
using RockPaperScissors.Api.Services.PlayerService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IIdGenerator, IdGenerator>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IMoveService, MoveService>();
builder.Services.AddScoped<IGameScoreService, GameScoreService>();

var connection = new SqliteConnection(builder.Configuration["ConnectionStrings:Game"]);
connection.Open();
builder.Services.AddDbContext<GameContext>(o => o.UseSqlite(connection));

builder.AddValidation();
builder.SetUpLogging();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGameEndpoints();

using var scope = app.Services.CreateScope();
await using var gameContext = scope.ServiceProvider.GetRequiredService<GameContext>();
await gameContext.Database.EnsureCreatedAsync();

app.Run();