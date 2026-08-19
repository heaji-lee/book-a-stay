using Microsoft.EntityFrameworkCore;
using BookAStay.Data;
using BookAStay.Repositories.Models;

namespace BookAStay.Repositories;

public class HotelsRepository {
    private readonly AppDbContext _context;

    public HotelsRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<List<HotelDto>> GetHotelsByName(string? name) {
        var searchName = name?.Trim();
        var query = _context.Hotels
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchName)) {
            var normalizedSearchName = searchName.ToLower();
            query = query.Where(hotel => hotel.Name.ToLower().Contains(normalizedSearchName));
        }

        return await query
            .Select(hotel => new HotelDto {
                Id = hotel.Id,
                Name = hotel.Name,
                Rooms = hotel.Rooms.Select(room => new RoomDto {
                    Id = room.Id,
                    Name = room.Type.ToString(),
                    Capacity = room.Capacity,
                    Price = room.Price
                }).ToList(),
                ImageUrl = hotel.ImageUrl
            })
            .ToListAsync();
    }

    public async Task<List<AvailableRoomDto>> GetAvailableRooms(
        DateTime checkInDate,
        DateTime checkOutDate,
        int guestCount,
        SortDirection sortDirection
    ) {
        var numberOfNights = (checkOutDate.Date - checkInDate.Date).Days;

        var query = _context.Rooms
            .AsNoTracking()
            .Where(room => room.Capacity >= guestCount)
            .Where(room => !room.Bookings.Any(booking =>
                booking.CheckInDate < checkOutDate &&
                booking.CheckOutDate > checkInDate))
            .Select(room => new AvailableRoomDto {
                HotelId = room.HotelId,
                HotelName = room.Hotel.Name,
                RoomId = room.Id,
                RoomType = room.Type.ToString(),
                Capacity = room.Capacity,
                TotalPrice = room.Price * numberOfNights
            });

        query = sortDirection == SortDirection.Descending
            ? query.OrderByDescending(room => room.TotalPrice)
            : query.OrderBy(room => room.TotalPrice);

        return await query
            .ToListAsync();
    }
}
