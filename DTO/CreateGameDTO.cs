namespace GameStore.API.DTO;
public record class CreateGameDTO(
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);