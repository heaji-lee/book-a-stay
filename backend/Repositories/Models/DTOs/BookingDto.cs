namespace BookAStay.Repositories.Models;

public class BookingDto {
    public Guid Id { get; set; }
    public string BookingReference { get; set; } = string.Empty;
    public int HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public int RoomId { get; set; }
    public RoomType RoomType { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public int GuestCount { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal TotalPrice { get; set; }
}