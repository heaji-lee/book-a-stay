# Book A Stay 🏖️


## Overview

The API allows a consumer to: 

- Find a hotel by name 
- Find available rooms between two dates for a given number of guests. 
- Book a room. 
- Retrieve booking details by booking reference number

It also exposes endpoints to seed and reset test data. 

## Tech Stack 👩🏻‍💻

- **ASP.NET Core (.NET 10)** - Web API
- **Entity Framework Core** - data access
- **SQL Server / Azure SQL** - production database
- **SQLite** - test database
- **Swagger** - API documentation, available at `/swagger` when running locally
- **xUnit** - automated tests, with an in-memory SQLite database for repository/service-level tests

## Getting started
**Prerequisites**

* [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download)

**Hosted Environment**

The API is deployed on Azure App Service. You can access the hosted Swagger UI [here](https://book-a-stay-cedxe5crghcdh9bt.ukwest-01.azurewebsites.net/swagger/index.html). 

**Running locally**

```bash
git clone https://github.com/heaji-lee/book-a-stay.git
cd backend
dotnet restore
dotnet run
```
The API will be available with Swagger UI at `http://localhost:5194/swagger`.

**Testing the API** 🧪

Before using the API, run the **POST** `Seed` endpoint to seed the database with the sample data. 
Once the database has been seeded, you can try the following endpoints. At any point,if you need to reset the data, run the **DELETE** `Reset` endpoint. 

* `GetHotelsByName` - Search for a hotel by name. 
        
    * Example: 
        `Bitz` - This will return the **Bitz Hotel** along with its rooms. 

* `GetAvailableRooms` - Search for rooms that are available within a given data range. 

    * Enter: 

        * **Check-in date** and **check-out date** in `YYYY-MM-DD` format. 
        * **Guest count**
        * **Sort direction**

            * `Ascending` (default, sorted by lowest price first)
            * `Descending` (sorted by highest price first)

    This endpoint returns all rooms that satisfy the search criteria. 

* `GetBookingByReference` - Retrieve an existing booking using its reference number. 

    * Example: `4FJ9K2` - This will return the booking for `Monica Geller`, including: 

        * Number of guests
        * Check-in and check-out dates
        * Total booking price
        * Room details
        * Name of the hotel

* `CreateBooking` - Create a new hotel booking. 

    * Use the following request body as a template: 

        ```json
        {
            "hotelId": 0,
            "roomId": 0,
            "guestName": "string",
            "guestCount": 0,
            "checkInDate": "YYYY-MM-DDT16:02:43.369Z",
            "checkOutDate": "YYYY-MM-DDT16:02:43.370Z"
        }
        ```
        Replicate the values with valid information. 
    
    If the booking cannot be created, the API returns a descriptive validation error explaining why (for example, the room is unavailable or the guest count exceeds the room capacity). 

    If the booking is successful, the API returns the booking details, including a unique booking reference that can later be used with the `GetBookingByReference` endpoint. 

## Running tests

```bash 
dotnet test book-a-stay.slnx
```

## Roadmap 🚀

Some features planned for future development include: 

* Build a front-end application 
* Add end-to-end testing
* Implement authentication and authorisation 
* Add pagination, filtering, and sorting to search hotels and available rooms
* Introduce logging and monitoring
* Deploy the API to a cloud provider
