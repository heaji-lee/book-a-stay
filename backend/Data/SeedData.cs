using BookAStay.Repositories.Models;

namespace BookAStay.Data;

public static class SeedData {
    public static List<Hotel> CreateHotels() {
        return [
            new Hotel {
                Name = "Park Heart Hotel",
                Rooms = [
                    new Room { Type = RoomType.Single, Capacity = 1, Price = 78.45m },
                    new Room { Type = RoomType.Single, Capacity = 1, Price = 85.51m },
                    new Room { Type = RoomType.Double, Capacity = 2, Price = 120.05m },
                    new Room { Type = RoomType.Double, Capacity = 2, Price = 125.78m },
                    new Room { Type = RoomType.Double, Capacity = 2, Price = 135.36m },
                    new Room { Type = RoomType.Deluxe, Capacity = 4, Price = 200.00m }
                ]
            },
            new Hotel {
                Name = "The Playa Hotel",
                Rooms = [
                    new Room { Type = RoomType.Single, Capacity = 1, Price = 96.20m },
                    new Room { Type = RoomType.Single, Capacity = 1, Price = 104.70m },
                    new Room { Type = RoomType.Double, Capacity = 2, Price = 148.90m },
                    new Room { Type = RoomType.Deluxe, Capacity = 3, Price = 162.30m },
                    new Room { Type = RoomType.Deluxe, Capacity = 4, Price = 178.60m },
                    new Room { Type = RoomType.Deluxe, Capacity = 4, Price = 245.00m }
                ]
            },
            new Hotel {
                Name = "Bitz Hotel",
                Rooms = [
                    new Room { Type = RoomType.Single, Capacity = 1, Price = 71.99m },
                    new Room { Type = RoomType.Double, Capacity = 2, Price = 112.50m },
                    new Room { Type = RoomType.Double, Capacity = 2, Price = 118.75m },
                    new Room { Type = RoomType.Double, Capacity = 2, Price = 127.20m },
                    new Room { Type = RoomType.Double, Capacity = 2, Price = 139.40m },
                    new Room { Type = RoomType.Deluxe, Capacity = 4, Price = 189.00m }
                ]
            },
            new Hotel {
                Name = "Four Seasonings Hotel",
                Rooms = [
                    new Room { Type = RoomType.Single, Capacity = 1, Price = 110.00m },
                    new Room { Type = RoomType.Single, Capacity = 1, Price = 119.75m },
                    new Room { Type = RoomType.Single, Capacity = 1, Price = 126.10m },
                    new Room { Type = RoomType.Double, Capacity = 2, Price = 165.00m },
                    new Room { Type = RoomType.Double, Capacity = 2, Price = 178.95m },
                    new Room { Type = RoomType.Deluxe, Capacity = 4, Price = 295.00m }
                ]
            },
            new Hotel {
                Name = "Tilhon Hotel",
                Rooms = [
                    new Room { Type = RoomType.Single, Capacity = 1, Price = 75.25m },
                    new Room { Type = RoomType.Single, Capacity = 1, Price = 83.90m },
                    new Room { Type = RoomType.Double, Capacity = 2, Price = 133.55m },
                    new Room { Type = RoomType.Deluxe, Capacity = 4, Price = 156.20m },
                    new Room { Type = RoomType.Deluxe, Capacity = 4, Price = 170.40m },
                    new Room { Type = RoomType.Deluxe, Capacity = 4, Price = 229.00m }
                ]
            },
            new Hotel {
                Name = "The Villa Hotel",
                Rooms = [
                    new Room { Type = RoomType.Single, Capacity = 1, Price = 88.005m },
                    new Room { Type = RoomType.Double, Capacity = 2, Price = 129.30m },
                    new Room { Type = RoomType.Double, Capacity = 2, Price = 141.80m },
                    new Room { Type = RoomType.Double, Capacity = 2, Price = 151.95m },
                    new Room { Type = RoomType.Double, Capacity = 2, Price = 165.40m },
                    new Room { Type = RoomType.Deluxe, Capacity = 4, Price = 261.35m }
                ]
            }
        ];
    }

    public static List<Booking> CreateBookings(List<Hotel> hotels) {
        return [
            new Booking {
                BookingReference = "7MD23A",
                HotelId = hotels[0].Id,
                RoomId = hotels[0].Rooms[0].Id,
                CheckInDate = DateTime.Today.AddDays(1),
                CheckOutDate = DateTime.Today.AddDays(3),
                GuestName = "Rachel Green",
                GuestCount = 1,
                TotalPrice = hotels[0].Rooms[0].Price * (DateTime.Today.AddDays(3) - DateTime.Today.AddDays(1)).Days
            },
            new Booking {
                BookingReference = "4FJ9K2",
                HotelId = hotels[1].Id,
                RoomId = hotels[1].Rooms[1].Id,
                CheckInDate = DateTime.Today.AddDays(5),
                CheckOutDate = DateTime.Today.AddDays(7),
                GuestName = "Monica Geller",
                GuestCount = 1,
                TotalPrice = hotels[1].Rooms[1].Price * (DateTime.Today.AddDays(7) - DateTime.Today.AddDays(5)).Days
            },
            new Booking {
                BookingReference = "K89IBU",
                HotelId = hotels[2].Id,
                RoomId = hotels[2].Rooms[2].Id,
                CheckInDate = DateTime.Today.AddDays(2),
                CheckOutDate = DateTime.Today.AddDays(9),
                GuestName = "Phoebe Buffay",
                GuestCount = 2,
                TotalPrice = hotels[2].Rooms[2].Price * (DateTime.Today.AddDays(9) - DateTime.Today.AddDays(2)).Days
            },
            new Booking {
                BookingReference = "NH5DM0",
                HotelId = hotels[3].Id,
                RoomId = hotels[3].Rooms[3].Id,
                CheckInDate = DateTime.Today.AddDays(8),
                CheckOutDate = DateTime.Today.AddDays(12),
                GuestName = "Joey Tribbiani",
                GuestCount = 2,
                TotalPrice = hotels[3].Rooms[3].Price * (DateTime.Today.AddDays(12) - DateTime.Today.AddDays(8)).Days
            },
            new Booking {
                BookingReference = "29KNI8",
                HotelId = hotels[4].Id,
                RoomId = hotels[4].Rooms[4].Id,
                CheckInDate = DateTime.Today.AddDays(1),
                CheckOutDate = DateTime.Today.AddDays(2),
                GuestName = "Chandler Bing",
                GuestCount = 3,
                TotalPrice = hotels[4].Rooms[4].Price * (DateTime.Today.AddDays(2) - DateTime.Today.AddDays(1)).Days
            },
            new Booking {
                BookingReference = "BDW56T",
                HotelId = hotels[5].Id,
                RoomId = hotels[5].Rooms[5].Id,
                CheckInDate = DateTime.Today.AddDays(4),
                CheckOutDate = DateTime.Today.AddDays(9),
                GuestName = "Ross Geller",
                GuestCount = 2,
                TotalPrice = hotels[5].Rooms[5].Price * (DateTime.Today.AddDays(9) - DateTime.Today.AddDays(4)).Days
            }
        ];
    }
}