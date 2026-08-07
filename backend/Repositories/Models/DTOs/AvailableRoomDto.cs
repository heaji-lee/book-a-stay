namespace BookAStay.Repositories.Models;

public class AvailableRoomDto {
    public required int HotelId { get; set; }
    public required string HotelName { get; set; }
    public required int RoomId { get; set; }
    public required string RoomType { get; set; }
    public required decimal TotalPrice { get; set; }
    public required int Capacity { get; set; }
}