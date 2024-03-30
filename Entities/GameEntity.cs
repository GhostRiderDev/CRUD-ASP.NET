namespace GameStore.API.Entities;

public class Game
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int IdGenre { get; set; }
    public Genre? Genre { set; get; }
    public decimal Price { set; get; }
    public DateOnly ReleaseDate { set; get; }
}