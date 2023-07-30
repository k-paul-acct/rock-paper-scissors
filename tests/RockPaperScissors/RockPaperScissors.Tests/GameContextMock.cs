using Microsoft.EntityFrameworkCore;
using RockPaperScissors.Api.Data;

namespace RockPaperScissors.Tests;

public class GameContextMock : GameContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("db");
    }
}