namespace BookAStay.Repositories.Models;

public class BookingRequestDto {
    public required int HotelId { get; set; }
    public required int RoomId { get; set; }
    public required string GuestName { get; set; }
    public required int GuestCount { get; set; }
    public required DateTime CheckInDate { get; set; }
    public required DateTime CheckOutDate { get; set; }
}