using GameStore.API.DTO;
using GameStore.API.Entities;

namespace GameStore.API.Mapping;

public static class GetMapping
{
    public static Game ToEntity(this CreateGameDTO gameDTO)
    {
        var (Name, IdGenre, Price, ReleaseDate) = gameDTO;
        return new()
        {
            Name = Name,
            IdGenre = IdGenre,
            Price = Price,
            ReleaseDate = ReleaseDate
        };
    }

    public static GameDTO ToDTO(this Game game)
    {
        return new(
                        game.Id,
                        game.Name,
                        game.Genre!.Name,
                        game.Price,
                        game.ReleaseDate);
    }
}