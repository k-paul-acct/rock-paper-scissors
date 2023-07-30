# Rock Paper Scissors Game API

*You can find Russian version [here](README_RU.md)*.

Welcome to the Rock Paper Scissors Game API! This project is a test task I completed for the Nexign Sprint. The API is
designed to allow users to play the classic Rock Paper Scissors game with various features. Below, I'll provide an
overview of the technologies used, game rules, and API endpoints.

## Technologies Used

- **ASP.NET Core with Minimal APIs** - The project uses the Minimal APIs feature of ASP.NET Core, making the codebase
  more concise and easier to maintain in comparison to old-fashion Controllers.
- **FluentValidation** - To ensure input data is valid and meets the required criteria, FluentValidation library is
  used for robust and flexible validation.
- **Serilog** - Logging is set up using Serilog library to track and monitor important events and exceptions during the
  game.
- **In-Memory SQLite** - An in-memory SQLite database is used to store data during the session.
- **Entity Framework Core** - The Entity Framework Core is employed for data access and management.
- **xUnit with AutoFixture** - The project uses xUnit testing framework along with AutoFixture for generating fake data.
  That is ensuring code quality and reliability.
- **Moq** - For test mocking, Moq library is utilized to simulate dependencies and isolate test cases effectively.
- **Docker** - The deployment process is streamlined with Docker, ensuring easy setup and reproducibility across
  different environments.

## Game Rules

- The game consists of five rounds, where players will make their moves in each round.
- Players can create a new game session, connect to an existing game by providing game's ID, and even choose to play
  with a bot as their opponent.
- At the end of the game, players can retrieve the results of all rounds, leave the game session, or restart the game
  for another session.
- Throughout the game, players have the option to retrieve live statistics showing the results of the passed rounds.
- The Game will end early if it is obvious who is the winner and who is the defeated.
- The game can be aborted if any of the players want to leave.

## API Endpoints

API endpoints have self-descriptive names and parameters as do error and responses. Here I give a brief description of
the endpoints. Full testing can be done in Swagger.

`POST /game/create` - Create a new game where you can choose to play with bot.

`POST /game/join` - Connect to an existing game by its ID.

`POST /game/move` - Make a move for the current round. Possible move values: `Rock`, `Paper`, `Scissors`.

`GET /game/{gameId}/stat` - Retrieve the results of all rounds in the end of a game.

`POST /game/restart` - Restart the game.

`POST /game/leave` - Leave the game.

`PUT /game/{gameId}/live` - Retrieve live statistics of the passed rounds while game is in process.

## Getting Started

To run the API on your local machine, follow these steps:

1. Install Docker and Docker Compose on your system (see official guides).
2. Clone this repository to your local machine.
3. Navigate to the project directory and, then, `src` directory.
4. Create `.env` file in the current directory (you can use the content of `.env.example` file) and specify values of
   environment variables if needed.
5. Run `docker compose up` to build and start the API.
6. The Swagger should now be accessible at http://localhost:8080/swagger (or other port, if you changed it). If you
   cannot see Swagger page, then manually set `ASPNETCORE_ENVIRONMENT="Development"` in `.env` file.

## Testing

To run the unit tests, execute the following command (you need .NET 7 SDK installed on your system) in
`tests/RockPaperScissors/RockPaperScissors.Tests` directory: `dotnet test`.

## Conclusion

Thank you for reviewing my Rock Paper Scissors Game API. I hope this demonstrates my proficiency in using C#,
ASP.NET Core, and various other technologies to create a robust and interactive API. If you have any questions or
feedback, please feel free to contact me at https://k-pavel.xyz.