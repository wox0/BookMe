namespace BookMe.Services;

using System.Text.RegularExpressions;
using Models;

public partial class AvailabilityService(DateParser dateParser)
{
    [GeneratedRegex(@"\(([^,]+), ([^,]+), ([^,]+)\)")]
    private static partial Regex AvailabilityRegex();

    public string CheckAvailability(string input, Hotel[] hotels, Booking[] bookings)
    {
        if(hotels.Length == 0)
            return "No available hotels";

        var match = AvailabilityRegex().Match(input);

        if (!IsInputValid(match))
            return "Wrong input. Try again..";

        var hotel = hotels.FirstOrDefault(h => h.Id == match.Groups[1].Value);

        if(hotel == null)
            return $"There is no hotel with id: {match.Groups[1].Value}";

        var roomType = match.Groups[3].Value;
        var hotelBookings = bookings.Where(b => b.HotelId == hotel.Id && b.RoomType == roomType).ToList();

        try
        {
            dateParser.ParseDateRange(match.Groups[2].Value, out var arrivalDate, out var departureDate);

            if(departureDate == DateTime.MinValue)
                departureDate = arrivalDate.AddDays(1);

            if(arrivalDate > departureDate)
                return "Departure date cannot be earlier than arrival date.";

            var bookedRooms = CountBookedRooms(hotelBookings, arrivalDate, departureDate);

            var freeRooms = hotel.Rooms.Count(r => r.RoomType == roomType) - bookedRooms;
            var oneRoom = freeRooms == 1;

            return $"There {(oneRoom ? "is" : "are")} {freeRooms} free room{(oneRoom ? "" : "s")}.";
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occured while parsing date: {e.Message}");
        }

        return "Try again...";
    }

    private int CountBookedRooms(List<Booking> bookings, DateTime arrivalDate, DateTime departureDate)
    {
        var bookedRooms = 0;

        foreach (var booking in bookings)
        {
            var bookingArrival = dateParser.Parse(booking.Arrival);
            var bookingDeparture = dateParser.Parse(booking.Departure);

            if (IsDateRangeOverlapped(arrivalDate, departureDate, bookingArrival, bookingDeparture))
                bookedRooms++;
        }

        return bookedRooms;
    }

    private bool IsDateRangeOverlapped(DateTime fromA, DateTime toA, DateTime fromB, DateTime toB) =>
        fromA < toB && fromB < toA;

    private bool IsInputValid(Match? match) =>
        match?.Groups is [_, { Value: not "" }, { Value: not "" }, { Value: not "" }];
}