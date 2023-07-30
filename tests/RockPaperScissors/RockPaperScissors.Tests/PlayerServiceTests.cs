using AutoFixture;
using RockPaperScissors.Api.Data;
using RockPaperScissors.Api.Data.Models;
using RockPaperScissors.Api.Services.IdGenerator;
using RockPaperScissors.Api.Services.PlayerService;
using RockPaperScissors.Api.Services.PlayerService.Results;
using Serilog;

namespace RockPaperScissors.Tests;

public class PlayerServiceTests
{
    private readonly PlayerService _sut;
    private readonly Mock<IIdGenerator> _idGenerator = new();
    private readonly GameContext _gameContext = new GameContextMock();
    private readonly Mock<ILogger> _logger = new();
    private readonly IFixture _fixture = new Fixture();

    public PlayerServiceTests()
    {
        _sut = new PlayerService(_idGenerator.Object, _gameContext, _logger.Object);
    }

    [Fact]
    public async void CreatePlayer()
    {
        var id = "id";
        var name = _fixture.Create<string>();
        _idGenerator.Setup(x => x.GeneratePlayerId()).Returns(id);

        var player = await _sut.CreatePlayer(name);

        Assert.IsType<Player>(player);
        Assert.Equal(id, player.Id);
        Assert.Equal(name, player.Name);
    }

    [Fact]
    public async void RequestGameRestart_WithWrongGameId()
    {
        var gameId = "g id";
        var playerId = "p id";
        _idGenerator.Setup(x => x.GeneratePlayerId()).Returns(playerId);

        var res1 = await _sut.RequestGameRestart(gameId, playerId);

        Assert.Equal(RequestGameRestartResult.GameNotFound, res1);
    }
}