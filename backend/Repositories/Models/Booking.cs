namespace BookAStay.Repositories.Models;

public class Booking {
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string BookingReference { get; set; }
    public int HotelId { get; set; }
    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;
    public required string GuestName { get; set; }
    public int GuestCount { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal TotalPrice { get; set; }
}
