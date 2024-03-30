using System.ComponentModel.DataAnnotations;

namespace GameStore.API.DTO;
public record class CreateGameDTO(
    [Required][StringLength(50)] string Name,
    int IdGenre,
    [Required][Range(1, 100)] decimal Price,
    [Required] DateOnly ReleaseDate
);