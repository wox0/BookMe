namespace BookMe.Tests;

using FluentAssertions;
using Models;
using Moq;
using Services;

public class ArgsResolverTests
{
    [Theory]
    [InlineData([new[] { "--hotels", "validPath", "--bookings", "validPath" }])]
    [InlineData([new[] { "--bookings", "validPath", "--hotels", "validPath" }])]
    public void ResolveArgs_ForValidArguments_ReturnsHotelsAndBookings(string[] args)
    {
        var fileReaderMock = new Mock<IFileReader>();
        fileReaderMock.Setup(r => r.IsValidPath(It.IsAny<string>())).Returns(true);
        fileReaderMock.Setup(r => r.ReadFile<Hotel[]>(It.IsAny<string>())).Returns(GetHotels());
        fileReaderMock.Setup(r => r.ReadFile<Booking[]>(It.IsAny<string>())).Returns(GetBookings());

        var argsResolver = new ArgsResolver(fileReaderMock.Object);

        argsResolver.ResolveArgs(args, out var hotels, out var bookings);

        hotels.Should().BeEquivalentTo(GetHotels());
        bookings.Should().BeEquivalentTo(GetBookings());
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