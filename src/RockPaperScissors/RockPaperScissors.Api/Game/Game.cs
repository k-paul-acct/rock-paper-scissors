using System.ComponentModel;
using RockPaperScissors.Api.Types.Enums;

namespace RockPaperScissors.Api.Game;

public class Game
{
    private const int RoundsNumber = 5;
    private readonly Turn[] _player1Turns = new Turn[RoundsNumber];
    private readonly Turn[] _player2Turns = new Turn[RoundsNumber];

    private int _roundsPassed;
    public required string Id { get; init; }

    public Player? Player1 { get; private set; }
    public Player? Player2 { get; private set; }

    public GameStatus Status { get; private set; } = GameStatus.PendingPlayers;

    public bool ContainsPlayer(string playerId)
    {
        return Player1?.Id == playerId || Player2?.Id == playerId;
    }

    private bool AllMadeTurn()
    {
        return _player1Turns[_roundsPassed] != Turn.None && _player2Turns[_roundsPassed] != Turn.None;
    }

    private void TryEndEarly()
    {
        var (first, second) = GetPlayersResult();
        var limit = RoundsNumber / 2 + 1;

        if (first >= limit || second >= limit) Status = GameStatus.Ended;
    }

    private MakeTurnResult MakeTurn(IList<Turn> turns, Turn turn)
    {
        if (turns[_roundsPassed] != Turn.None) return MakeTurnResult.AlreadyMade;

        turns[_roundsPassed] = turn;

        if (AllMadeTurn())
        {
            ++_roundsPassed;
            TryEndEarly();
        }

        if (_roundsPassed == RoundsNumber) Status = GameStatus.Ended;

        return MakeTurnResult.Success;
    }

    public AddPlayerResult AddPlayer(Player player)
    {
        if (Status != GameStatus.PendingPlayers) return AddPlayerResult.AlreadyMaxPlayersCount;

        if (Player1 is null)
        {
            Player1 = player;
            return AddPlayerResult.Success;
        }

        if (Player2 is null)
        {
            Player2 = player;

            Status = GameStatus.InProcess;

            return AddPlayerResult.Success;
        }

        return AddPlayerResult.AlreadyMaxPlayersCount;
    }

    private MakeTurnResult MakeTurnById(string playerId, Turn turn)
    {
        if (playerId == Player1!.Id) return MakeTurn(_player1Turns, turn);

        if (playerId == Player2!.Id) return MakeTurn(_player2Turns, turn);

        return MakeTurnResult.NoSuchPlayer;
    }

    public MakeTurnResult MakeTurn(string playerId, Turn turn)
    {
        if (turn == Turn.None) return MakeTurnResult.InvalidTurn;

        return Status switch
        {
            GameStatus.InProcess => MakeTurnById(playerId, turn),
            GameStatus.Aborted => MakeTurnResult.GameAborted,
            GameStatus.Ended => MakeTurnResult.GameEnded,
            GameStatus.PendingPlayers => MakeTurnResult.PendingPlayers,
            _ => throw new InvalidEnumArgumentException()
        };
    }

    public LeaveReslt Leave(string playerId)
    {
        if (playerId != Player1?.Id && playerId != Player2?.Id) return LeaveReslt.NoSuchPlayer;

        Player1 = null;
        Player2 = null;
        Status = GameStatus.Aborted;

        return LeaveReslt.Success;
    }

    public void Restart()
    {
        _roundsPassed = 0;
        Array.Fill(_player1Turns, Turn.None);
        Array.Fill(_player2Turns, Turn.None);
        Status = GameStatus.PendingPlayers;
    }

    private (int, int) GetRoundResult(int roundIndex)
    {
        var first = _player1Turns[roundIndex];
        var second = _player2Turns[roundIndex];

        var points = (first, second) switch
        {
            (Turn.Rock, Turn.Paper) => (0, 1),
            (Turn.Rock, Turn.Scissors) => (1, 0),
            (Turn.Paper, Turn.Rock) => (1, 0),
            (Turn.Paper, Turn.Scissors) => (0, 1),
            (Turn.Scissors, Turn.Rock) => (0, 1),
            (Turn.Scissors, Turn.Paper) => (1, 0),
            _ => (1, 1)
        };

        return points;
    }

    private (int, int) GetPlayersResult()
    {
        var firstSum = 0;
        var secondSum = 0;

        for (var i = 0; i < _roundsPassed; ++i)
        {
            var (firstRes, secondRes) = GetRoundResult(i);
            firstSum += firstRes;
            secondSum += secondRes;
        }

        return (firstSum, secondSum);
    }

    public GameResult? GetResult()
    {
        if (Status != GameStatus.Ended) return null;

        var (firstRes, secondRes) = GetPlayersResult();
        var firstStatus = PlayerStatus.Tie;
        var secondStatus = PlayerStatus.Tie;
        if (firstRes > secondRes)
        {
            firstStatus = PlayerStatus.Winner;
            secondStatus = PlayerStatus.Defeated;
        }
        else if (secondRes > firstRes)
        {
            secondStatus = PlayerStatus.Winner;
            firstStatus = PlayerStatus.Defeated;
        }

        var result = new GameResult
        {
            Players = new PlayerResult[]
            {
                new(Player1!, _player1Turns, firstRes, firstStatus),
                new(Player2!, _player2Turns, secondRes, secondStatus)
            }
        };

        return result;
    }

    public GameLive? GetLive()
    {
        if (Status != GameStatus.InProcess) return null;

        return new GameLive
        {
            CurrentRound = _roundsPassed + 1,
            Players = new PlayerLive[]
            {
                new(Player1!, _player1Turns[.._roundsPassed]),
                new(Player2!, _player2Turns[.._roundsPassed])
            }
        };
    }
}