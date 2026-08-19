namespace BookAStay.Repositories.Models;

public class HotelDto {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<RoomDto> Rooms { get; set; } = new List<RoomDto>();
    public string ImageUrl { get; set; } = string.Empty;
}