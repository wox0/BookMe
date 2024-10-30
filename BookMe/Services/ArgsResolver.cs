namespace BookMe.Services;
using Models;

public class ArgsResolver(IFileReader reader)
{
    private const string HotelsArgName = "--hotels";
    private const string BookingsArgName = "--bookings";

    public void ResolveArgs(string[] args, out Hotel[] hotels, out Booking[] bookings)
    {
        hotels = [];
        bookings = [];

        if (TryGetArgValue(HotelsArgName, args, out var hotelsArgValue))
        {
            var output = reader.ReadFile<Hotel[]>(hotelsArgValue);
            hotels = output ?? [];
        }

        if (TryGetArgValue(BookingsArgName, args, out var bookingsArgValue))
        {
            var output = reader.ReadFile<Booking[]>(bookingsArgValue);
            bookings = output ?? [];
        }
    }

    private bool TryGetArgValue(string argExpectedValue, string[] incomingArgs, out string argValue)
    {
        if (incomingArgs.Contains(argExpectedValue))
        {
            var i = Array.IndexOf(incomingArgs, argExpectedValue);
            var valueIndex = i + 1;

            if (incomingArgs.Length > valueIndex && incomingArgs[valueIndex] != string.Empty && reader.IsValidPath(incomingArgs[valueIndex]))
            {
                argValue = incomingArgs[valueIndex];
                return true;
            }
        }

        argValue = string.Empty;
        return false;
    }
}