namespace BookAStay.Repositories.Models;

public class RoomDto {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public decimal Price { get; set; }
}