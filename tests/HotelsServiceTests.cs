using BookAStay.Data;
using BookAStay.Repositories;
using BookAStay.Repositories.Models;
using BookAStay.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace tests;

public class HotelsServiceTests {
    [Fact]
    public async Task GetHotelsByName_ShouldReturnAllHotels_WhenNameIsNull() {
        var (_, service) = await CreateServiceAsync();

        var result = await service.GetHotelsByName(null);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetHotelsByName_ShouldFilterCaseInsensitively() {
        var (_, service) = await CreateServiceAsync();

        var result = await service.GetHotelsByName("elen");

        Assert.Single(result);
        Assert.Equal("Helen's Hotel", result[0].Name);
    }

    [Fact]
    public async Task GetHotelsByName_ShouldReturnEmptyList_WhenNoMatch() {
        var (_, service) = await CreateServiceAsync();

        var result = await service.GetHotelsByName("does-not-exist");

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAvailableRooms_ShouldExcludeRoomsBelowCapacity() {
        var (_, service) = await CreateServiceAsync();

        var result = await service.GetAvailableRooms(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), 3, SortDirection.Ascending);

        Assert.DoesNotContain(result, room => room.RoomType == "Single");
    }

    [Fact]
    public async Task GetAvailableRooms_ShouldExcludeRoomsWithOverlappingBookings() {
        var (context, service) = await CreateServiceAsync();

        var room = context.Rooms.First();
        context.Bookings.Add(new Booking {
            BookingReference = "OVERLAP",
            HotelId = room.HotelId,
            RoomId = room.Id,
            Room = room,
            GuestName = "Alice Wonderland",
            GuestCount = 1,
            CheckInDate = DateTime.Today.AddDays(1),
            CheckOutDate = DateTime.Today.AddDays(4),
            TotalPrice = 100m
        });

        await context.SaveChangesAsync();

        var result = await service.GetAvailableRooms(DateTime.Today.AddDays(2), DateTime.Today.AddDays(3), 1, SortDirection.Ascending);

        Assert.DoesNotContain(result, availableRoom => availableRoom.RoomId == room.Id);
    }

    [Fact]
    public async Task GetAvailableRooms_ShouldIncludeRoomsWithNonOverlappingBookings() {
        var (context, service) = await CreateServiceAsync();

        var room = context.Rooms.First();
        context.Bookings.Add(new Booking {
            BookingReference = "OVERLAP",
            HotelId = room.HotelId,
            RoomId = room.Id,
            Room = room,
            GuestName = "Alice Wonderland",
            GuestCount = 1,
            CheckInDate = DateTime.Today.AddDays(1),
            CheckOutDate = DateTime.Today.AddDays(2),
            TotalPrice = 100m
        });

        await context.SaveChangesAsync();

        var result = await service.GetAvailableRooms(DateTime.Today.AddDays(2), DateTime.Today.AddDays(3), 1, SortDirection.Ascending);

        Assert.Contains(result, availableRoom => availableRoom.RoomId == room.Id);
    }

    [Fact]
    public async Task GetAvailableRooms_ShouldSortAscendingByDefault() {
        var (_, service) = await CreateServiceAsync();

        var result = await service.GetAvailableRooms(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), 1, SortDirection.Ascending);

        Assert.True(result.SequenceEqual(result.OrderBy(r => r.TotalPrice)));
    }

    [Fact]
    public async Task GetAvailableRooms_ShouldSortDescendingWhenSpecified() {
        var (_, service) = await CreateServiceAsync();

        var result = await service.GetAvailableRooms(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), 1, SortDirection.Descending);

        Assert.True(result.SequenceEqual(result.OrderByDescending(r => r.TotalPrice)));
    }

    [Fact]
    public async Task GetAvailableRooms_ShouldCalculateTotalPriceForCorrectNumberOfNights() {
        var (context, service) = await CreateServiceAsync();

        var room = context.Rooms.First(r => r.Type == RoomType.Single && r.Hotel!.Name == "Helen's Hotel");
        var result = await service.GetAvailableRooms(DateTime.Today.AddDays(1), DateTime.Today.AddDays(4), 1, SortDirection.Ascending);

        var availableRoom = Assert.Single(result, r => r.RoomId == room.Id);
        Assert.Equal(3 * 100m, availableRoom.TotalPrice);
    }

    private static async Task<(AppDbContext Context, HotelsService Service)> CreateServiceAsync() {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var hotel1 = new Hotel { Name = "Helen's Hotel" };
        var hotel2 = new Hotel { Name = "Lee's Hotel" };

        var singleRoom = new Room { Hotel = hotel1, Type = RoomType.Single, Capacity = 1, Price = 100m };
        var doubleRoom = new Room { Hotel = hotel1, Type = RoomType.Double, Capacity = 2, Price = 150m };
        var singleRoom2 = new Room { Hotel = hotel2, Type = RoomType.Single, Capacity = 1, Price = 120m };

        hotel1.Rooms.Add(singleRoom);
        hotel1.Rooms.Add(doubleRoom);
        hotel2.Rooms.Add(singleRoom2);

        context.Hotels.AddRange(hotel1, hotel2);
        await context.SaveChangesAsync();

        var repository = new HotelsRepository(context);
        var service = new HotelsService(repository);

        return (context, service);
    }
}
