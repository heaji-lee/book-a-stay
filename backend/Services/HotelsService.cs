using BookAStay.Repositories;
using BookAStay.Repositories.Models;

namespace BookAStay.Services;

public class HotelsService(HotelsRepository hotelsRepository) {
    public async Task<List<HotelDto>> GetHotelsByName(string? name) {
        return await hotelsRepository.GetHotelsByName(name);
    }

    public async Task<List<AvailableRoomDto>> GetAvailableRooms(
        DateTime checkInDate,
        DateTime checkOutDate,
        int guestCount,
        SortDirection sortDirection
    ) {
        return await hotelsRepository.GetAvailableRooms(checkInDate, checkOutDate, guestCount, sortDirection);
    }
}
