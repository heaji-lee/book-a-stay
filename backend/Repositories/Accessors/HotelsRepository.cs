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
                }).ToList()
            })
            .ToListAsync();
    }
}
