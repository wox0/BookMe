namespace BookMe.Tests;

using FluentAssertions;
using Models;
using Services;

public class AvailabilityServiceTests
{
    [Theory]
    [MemberData(nameof(GetValidData))]
    public void CheckAvailability_ForValidInput_ReturnsCorrectFreeRoomsAmount(string input, Hotel[] hotels, Booking[] bookings, string expectedOutput)
    {
        var availabilityService = new AvailabilityService(new DateParser());

        var result = availabilityService.CheckAvailability(input, hotels, bookings);

        result.Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(GetInvalidData))]
    public void CheckAvailability_ForInvalidInput_ReturnsExpectedErrorMessage(string input, Hotel[] hotels, Booking[] bookings, string expectedOutput)
    {
        var availabilityService = new AvailabilityService(new DateParser());

        var result = availabilityService.CheckAvailability(input, hotels, bookings);

        result.Should().Be(expectedOutput);
    }

    public static IEnumerable<object[]> GetInvalidData()
    {
        yield return [ "Availability(H1, SGL)", GetHotels(), GetBookings(), "Wrong input. Try again.." ];
        yield return [ "Availability(H1,, SGL)", GetHotels(), GetBookings(), "Wrong input. Try again.." ];
        yield return [ "Availability(H1, 20240829, SGL)", Array.Empty<Hotel>(), GetBookings(), "No available hotels" ];
        yield return [ "Availability(H1, 20241010-12323321, SGL)", GetHotels(), GetBookings(), "Try again..." ];
        yield return [ "Availability(H1, 20241010-20231010, SGL)", GetHotels(), GetBookings(), "Departure date cannot be earlier than arrival date." ];
        yield return [ "Availability(H2, 20240901-20240903, SGL)", GetHotels(), GetBookings(), "There is no hotel with id: H2" ];
        yield return [ "aaa", GetHotels(), GetBookings(), "Wrong input. Try again.." ];
        yield return [ "", GetHotels(), GetBookings(), "Wrong input. Try again.." ];
        yield return [ "H1 20240902 DBL", GetHotels(), GetBookings(), "Wrong input. Try again.." ];
    }


    public static IEnumerable<object[]> GetValidData()
    {
        yield return [ "Availability(H1, 20240901, SGL)", GetHotels(), GetBookings(), "There is 1 free room." ];
        yield return [ "Availability(H1, 20240904, SGL)", GetHotels(), GetBookings(), "There is 1 free room." ];
        yield return [ "Availability(H1, 20240829, SGL)", GetHotels(), GetBookings(), "There are 2 free rooms." ];
        yield return [ "Availability(H1, 20241010, SGL)", GetHotels(), GetBookings(), "There are 2 free rooms." ];
        yield return [ "Availability(H1, 20241010, SGL)", GetHotels(), Array.Empty<Booking>(), "There are 2 free rooms." ];
        yield return [ "Availability(H1, 20240901-20240903, SGL)", GetHotels(), GetBookings(), "There is 1 free room." ];
        yield return [ "Availability(H1, 20240904-20241001, DBL)", GetHotels(), GetBookings(), "There are 2 free rooms." ];
        yield return [ "Availability(H1, 20240904, DBL)", GetHotels(), Array.Empty<Booking>(), "There are 2 free rooms." ];
        yield return [ "Availability(H1, 20240904, DBL)", GetHotels(), GetBookings(), "There are 2 free rooms." ];
        yield return [ "Availability(H1, 20240902, DBL)", GetHotels(), GetBookings(), "There is 1 free room." ];
    }

    private static Hotel[] GetHotels() =>
    [
        new()
        {
            Id = "H1",
            Rooms =
            [
                new Room { RoomType = "SGL" },
                new Room { RoomType = "SGL" },
                new Room { RoomType = "DBL" },
                new Room { RoomType = "DBL" },
            ],
        }
    ];

    private static Booking[] GetBookings() =>
    [
        new() { HotelId = "H1", Arrival = "20240901", Departure = "20240904", RoomType = "SGL" },
        new() { HotelId = "H1", Arrival = "20240903", Departure = "20240905", RoomType = "SGL" },
        new() { HotelId = "H1", Arrival = "20240902", Departure = "20240904", RoomType = "DBL" }
    ];
}