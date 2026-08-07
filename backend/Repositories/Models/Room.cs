namespace BookAStay.Repositories.Models;

public enum RoomType {
    Single,
    Double,
    Deluxe
}

public class Room {
    public int Id { get; set; }
    public int HotelId { get; set; }
    public Hotel Hotel { get; set; } = null!;
    public RoomType Type { get; set; }
    public decimal Price { get; set; }
    public int Capacity { get; set; }
    public List<Booking> Bookings { get; set; } = [];
}