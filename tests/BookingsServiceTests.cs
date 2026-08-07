using BookAStay.Data;
using BookAStay.Repositories;
using BookAStay.Repositories.Models;
using BookAStay.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace tests;

public class BookingsServiceTests {
    [Fact]
    public async Task CreateBooking_ShouldSucceed_WhenRequestIsValid() {
        var (context, service, _) = await CreateServiceAsync();

        var result = await service.CreateBooking(
            hotelId: 1,
            roomId: 1,
            guestName: "Alice Wonderland",
            guestCount: 1,
            checkInDate: DateTime.Today.AddDays(1),
            checkOutDate: DateTime.Today.AddDays(3));

        Assert.NotNull(result);
        Assert.Equal("Alice Wonderland", result.GuestName);
        Assert.Equal(1, result.GuestCount);
        Assert.False(string.IsNullOrWhiteSpace(result.BookingReference));

        var bookingCount = await context.Bookings.CountAsync();
        Assert.Equal(1, bookingCount);
    }

    [Fact]
    public async Task CreateBooking_ShouldThrow_WhenCheckInDateIsInThePast() {
        var (_, service, _) = await CreateServiceAsync();

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateBooking(
            hotelId: 1,
            roomId: 1,
            guestName: "Alice Wonderland",
            guestCount: 1,
            checkInDate: DateTime.Today.AddDays(-1),
            checkOutDate: DateTime.Today.AddDays(1)));

        Assert.Contains("past", ex.Message);
    }

    [Fact]
    public async Task CreateBooking_ShouldThrow_WhenRoomIsAlreadyBookedForThoseDates() {
        var (context, service, room) = await CreateServiceAsync();

        context.Bookings.Add(new Booking {
            BookingReference = "EXISTING",
            HotelId = 1,
            RoomId = room.Id,
            Room = room,
            GuestName = "Bob",
            GuestCount = 1,
            CheckInDate = DateTime.Today.AddDays(1),
            CheckOutDate = DateTime.Today.AddDays(3),
            TotalPrice = 200m
        });

        await context.SaveChangesAsync();

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateBooking(
            hotelId: 1,
            roomId: 1,
            guestName: "Alice Wonderland",
            guestCount: 1,
            checkInDate: DateTime.Today.AddDays(2),
            checkOutDate: DateTime.Today.AddDays(4)));

        Assert.Contains("already booked", ex.Message);
    }

    [Fact]
    public async Task CreateBooking_ShouldThrow_WhenGuestCountExceedsCapacity() {
        var (_, service, _) = await CreateServiceAsync();

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateBooking(
            hotelId: 1,
            roomId: 1,
            guestName: "Alice Wonderland",
            guestCount: 3,
            checkInDate: DateTime.Today.AddDays(1),
            checkOutDate: DateTime.Today.AddDays(3)));

        Assert.Contains("cannot accommodate", ex.Message);
    }

    [Fact]
    public async Task CreateBooking_ShouldThrow_WhenCheckOutBeforeOrEqualCheckIn() {
        var (_, service, _) = await CreateServiceAsync();

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateBooking(
            hotelId: 1,
            roomId: 1,
            guestName: "Alice Wonderland",
            guestCount: 1,
            checkInDate: DateTime.Today.AddDays(2),
            checkOutDate: DateTime.Today.AddDays(2)));

        Assert.Contains("before", ex.Message);
    }

    [Fact]
    public async Task CreateBooking_ShouldThrow_WhenGuestNameIsEmpty() {
        var (_, service, _) = await CreateServiceAsync();

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateBooking(
            hotelId: 1,
            roomId: 1,
            guestName: "   ",
            guestCount: 1,
            checkInDate: DateTime.Today.AddDays(1),
            checkOutDate: DateTime.Today.AddDays(3)));

        Assert.Contains("Guest name", ex.Message);
    }

    [Fact]
    public async Task CreateBooking_ShouldThrow_WhenRoomDoesNotBelongToHotel() {
        var (_, service, _) = await CreateServiceAsync();

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateBooking(
            hotelId: 999,
            roomId: 1,
            guestName: "Alice Wonderland",
            guestCount: 1,
            checkInDate: DateTime.Today.AddDays(1),
            checkOutDate: DateTime.Today.AddDays(3)));

        Assert.Contains("does not belong", ex.Message);
    }

    [Fact]
    public async Task CreateBooking_ShouldThrow_WhenRoomDoesNotExist() {
        var (_, service, _) = await CreateServiceAsync();

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateBooking(
            hotelId: 1,
            roomId: 999,
            guestName: "Alice Wonderland",
            guestCount: 1,
            checkInDate: DateTime.Today.AddDays(1),
            checkOutDate: DateTime.Today.AddDays(3)));

        Assert.Contains("not available", ex.Message);
    }

    [Fact]
    public async Task CreateBooking_ShouldSucceed_WhenNewBookingStartsExactlyOnPreviousCheckoutDate() {
        var (context, service, room) = await CreateServiceAsync();

        context.Bookings.Add(new Booking {
            BookingReference = "EXISTING",
            HotelId = 1,
            RoomId = room.Id,
            Room = room,
            GuestName = "Bob",
            GuestCount = 1,
            CheckInDate = DateTime.Today.AddDays(1),
            CheckOutDate = DateTime.Today.AddDays(3),
            TotalPrice = 200m
        });

        await context.SaveChangesAsync();

        var result = await service.CreateBooking(
            hotelId: 1,
            roomId: 1,
            guestName: "Alice Wonderland",
            guestCount: 1,
            checkInDate: DateTime.Today.AddDays(3),
            checkOutDate: DateTime.Today.AddDays(5));

        Assert.NotNull(result);
        Assert.Equal("Alice Wonderland", result.GuestName);
    }

    [Fact]
    public async Task GetBooking_ShouldReturnNull_WhenReferenceDoesNotExist() {
        var (_, service, _) = await CreateServiceAsync();

        var result = await service.GetBooking("DOES_NOT_EXIST");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetBooking_ShouldReturnCorrectBooking_WhenReferenceExists() {
        var (context, service, room) = await CreateServiceAsync();

        var booking = new Booking {
            BookingReference = "ABC123",
            HotelId = 1,
            RoomId = room.Id,
            Room = room,
            GuestName = "Alice Wonderland",
            GuestCount = 1,
            CheckInDate = DateTime.Today.AddDays(1),
            CheckOutDate = DateTime.Today.AddDays(3),
            TotalPrice = 200m
        };

        context.Bookings.Add(booking);
        await context.SaveChangesAsync();

        var result = await service.GetBooking("ABC123");

        Assert.NotNull(result);
        Assert.Equal("ABC123", result.BookingReference);
        Assert.Equal("Alice Wonderland", result.GuestName);
    }

    private static async Task<(AppDbContext Context, BookingsService Service, Room Room)> CreateServiceAsync() {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var hotel = new Hotel {
            Name = "Test Hotel"
        };

        var room = new Room {
            Hotel = hotel,
            Type = RoomType.Single,
            Capacity = 2,
            Price = 100m
        };

        hotel.Rooms.Add(room);
        context.Hotels.Add(hotel);
        await context.SaveChangesAsync();

        var repository = new BookingsRepository(context);
        var service = new BookingsService(repository);

        return (context, service, room);
    }
}
