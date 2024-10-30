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
            return "No available hotels.";

        if (!TryMatchInput(input, out var groups))
            return "Wrong input. Try again..";

        var hotelId = groups[1].Value;
        var dateRange = groups[2].Value;
        var roomType = groups[3].Value;

        var hotel = hotels.FirstOrDefault(h => h.Id == hotelId);

        if(hotel == null)
            return $"There is no hotel with id: {hotelId}.";

        if (hotel.RoomTypes.All(t => t.Code != roomType))
            return $"Hotel with id {hotelId} has no room type of {roomType}.";

        var hotelBookings = bookings.Where(b => b.HotelId == hotel.Id && b.RoomType == roomType).ToList();

        if (hotelBookings.Count == 0)
        {
            var hotelRooms = hotel.Rooms.Count(r => r.RoomType == roomType);
            return GetOutputMessage(hotelRooms == 1, hotelRooms);
        }

        DateTime arrivalDate;
        DateTime departureDate;

        try
        {
            dateParser.ParseDateRange(dateRange, out arrivalDate, out departureDate);
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occured while parsing date: {e.Message}");
            return "Try again...";
        }

        if(departureDate == DateTime.MinValue)
            departureDate = arrivalDate.AddDays(1);

        if(arrivalDate > departureDate)
            return "Departure date cannot be earlier than arrival date.";

        var bookedRooms = CountBookedRooms(hotelBookings, arrivalDate, departureDate);
        var freeRooms = hotel.Rooms.Count(r => r.RoomType == roomType) - bookedRooms;

        return GetOutputMessage(freeRooms == 1, freeRooms);
    }

    private bool TryMatchInput(string input, out GroupCollection groups)
    {
        var match = AvailabilityRegex().Match(input);

        groups = match.Groups;

        return IsInputValid(match);
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

    private string GetOutputMessage(bool isOneRoom, int roomsAmount) =>
        $"There {(isOneRoom ? "is" : "are")} {roomsAmount} free room{(isOneRoom ? "" : "s")}.";

    private bool IsDateRangeOverlapped(DateTime fromA, DateTime toA, DateTime fromB, DateTime toB) =>
        fromA < toB && fromB < toA;

    private bool IsInputValid(Match? match) =>
        match?.Groups is [_, { Value: not "" }, { Value: not "" }, { Value: not "" }];
}