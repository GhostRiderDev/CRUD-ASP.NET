using GameStore.API.DTO;

namespace GameStore.API.Router;

public static class GameRouter
{
    private static readonly List<GameDTO> gameDTOs = [
        new GameDTO(1, "The red night red", "Action", 9.99M, new DateOnly(2004, 4, 5)),
        new GameDTO(2, "The gray night blue", "Adventure", 19.99M, new DateOnly(2005, 5, 6)),
        new GameDTO(3, "The green night green", "RPG", 29.99M, new DateOnly(2006, 6, 7)),
        new GameDTO(4, "The blue night yellow", "Strategy", 39.99M, new DateOnly(2007, 7, 8))
    ];

    public static RouteGroupBuilder MapGameRouter(this WebApplication app)
    {
        var group = app.MapGroup("/api/games").WithParameterValidation();

        group.MapGet("", () => gameDTOs);
        group.MapGet("/{id}", (int id) =>
        {
            GameDTO? gameDB = gameDTOs.Find(game => game.Id == id);
            return gameDB is null ? Results.NotFound() : Results.Ok(gameDB);
        })
           .WithName("GetGame");
        group.MapPost("", (CreateGameDTO newGame) =>
        {
            GameDTO gameDTO = new(
                gameDTOs.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate);
            gameDTOs.Add(gameDTO);
            return Results.CreatedAtRoute("GetGame", new { id = gameDTO.Id, gameDTO });
        }).WithParameterValidation();

        group.MapPut("/{id}", (int id, UpdateGameDTO updateGameDTO) =>
        {
            var index = gameDTOs.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

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

        group.MapDelete("/{id}", (int id) =>
        {
            gameDTOs.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        });

        return group;
    }

}