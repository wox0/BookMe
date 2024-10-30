namespace BookMe.Tests;

using FluentAssertions;
using Models;
using Services;

public class JsonFileReaderTests
{
    [Fact]
    public void ReadFile_ForValidHotelJsonFile_ReturnsCorrectHotelsArrayObject()
    {
        var reader = new JsonFileReader();
        var output = reader.ReadFile<Hotel[]>(GetTestDataPath("TestHotels.json"));

        output.Should().BeEquivalentTo(GetHotel());
    }

    [Fact]
    public void ReadFile_ForValidBookingJsonFile_ReturnsCorrectBookingsArrayObject()
    {
        var reader = new JsonFileReader();
        var output = reader.ReadFile<Booking[]>(GetTestDataPath("TestBookings.json"));

        output.Should().BeEquivalentTo(GetBooking());
    }

    [Fact]
    public void ReadFile_ForInvalidBookingJsonFile_ReturnsEmptyObject()
    {
        var reader = new JsonFileReader();
        var output = reader.ReadFile<Booking[]>(GetTestDataPath("TestHotels.json"));

        output.Should().BeEquivalentTo([new Booking()]);
    }

    [Fact]
    public void ReadFile_ForInvalidFilePath_ReturnsNull()
    {
        var reader = new JsonFileReader();
        var output = reader.ReadFile<Booking[]>(GetTestDataPath("invalid.json"));

        output.Should().BeNull();
    }

    private string GetTestDataPath(string fileName)
    {
        var workingDirectory = Environment.CurrentDirectory;
        var projectDirectory = Directory.GetParent(workingDirectory)!.Parent.Parent.FullName;
        return $@"{projectDirectory}\TestData\{fileName}";
    }

    private static Hotel[] GetHotel() =>
    [
        new()
        {
            Id = "H1",
            Name = "Hotel California",
            RoomTypes =
            [
                new RoomTypes
                    {
                        Code = "SGL",
                        Description = "Single Room",
                        Amenities = [ "WiFi", "TV" ],
                        Features = [ "Non-smoking" ]
                    }
            ],
            Rooms =
            [
                new Room { RoomType = "SGL", RoomId = "101"}
            ]
        }
    ];

    private static Booking[] GetBooking() =>
    [
        new()
        {
            HotelId = "H1",
            Arrival = "20240901",
            Departure = "20240903",
            RoomType = "DBL",
            RoomRate = "Prepaid"
        }
    ];
}