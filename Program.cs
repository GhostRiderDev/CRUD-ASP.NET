using GameStore.API;
using GameStore.API.DTO;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDTO> gameDTOs = [
    new GameDTO(1, "The red night red", "Action", 9.99M, new DateOnly(2004, 4, 5)),
    new GameDTO(2, "The blue night blue", "Adventure", 19.99M, new DateOnly(2005, 5, 6)),
    new GameDTO(3, "The green night green", "RPG", 29.99M, new DateOnly(2006, 6, 7)),
    new GameDTO(4, "The yellow night yellow", "Strategy", 39.99M, new DateOnly(2007, 7, 8))
];

app.MapGet("/", () => "Hello World!");
app.MapGet("/cos4h", () => "Hi, I amd cos4h");
app.MapGet("/api/games", () => gameDTOs);
app.MapGet("/api/games/{id}", (int id) =>
{
    GameDTO? gameDB = gameDTOs.Find(game => game.Id == id);
    return gameDB is null ? Results.NotFound() : Results.Ok(gameDB);
})
   .WithName("GetGame");
app.MapPost("/api/game", (CreateGameDTO newGame) =>
{
    GameDTO gameDTO = new(
        gameDTOs.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate);
    gameDTOs.Add(gameDTO);
    return Results.CreatedAtRoute("GetGame", new { id = gameDTO.Id, gameDTO });
});

app.MapPut("/api/game/{id}", (int id, UpdateGameDTO updateGameDTO) =>
{
    var index = gameDTOs.FindIndex(game => game.Id == id);
    var (Name, Genre, Price, ReleaseDate) = updateGameDTO;
    gameDTOs[index] = new GameDTO(
        id,
        Name,
        Genre,
        Price,
        ReleaseDate
    );
    return Results.NoContent();
});

app.MapDelete("/api/games/{id}", (int id) =>
{
    gameDTOs.RemoveAll(game => game.Id == id);
    return Results.NoContent();
});

app.Run();
