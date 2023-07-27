FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY RockPaperScissors.Api/RockPaperScissors.Api.csproj RockPaperScissors.Api/
RUN dotnet restore "RockPaperScissors.Api/RockPaperScissors.Api.csproj"
COPY . .
WORKDIR /src/RockPaperScissors.Api
RUN dotnet build "RockPaperScissors.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RockPaperScissors.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT dotnet RockPaperScissors.Api.dll
