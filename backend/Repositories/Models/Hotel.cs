namespace BookAStay.Repositories.Models;

public class Hotel {
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Room> Rooms { get; set; } = [];
    public string ImageUrl { get; set; } = string.Empty;
}
