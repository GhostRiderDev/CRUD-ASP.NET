using GameStore.API.Data;
using GameStore.API.DTO;
using GameStore.API.Entities;
using GameStore.API.Mapping;
using Microsoft.EntityFrameworkCore;

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

        group.MapGet("", async (GameStoreContext dbConteext) =>
           await dbConteext.Games.Include(game => game.Genre)
                            .Select(game => game.ToDTO())
                            .AsNoTracking()
                            .ToListAsync()
        );
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Game? gameDB = await dbContext.Games.FindAsync(id);
            return gameDB is null ? Results.NotFound() : Results.Ok(gameDB);
        })
           .WithName("GetGame");
        group.MapPost("", async (CreateGameDTO newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game.ToDTO());
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

        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games
            .Where(game => game.Id == id)
            .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return group;
    }

}