namespace BookMe;

using Services;

public class App(ArgsResolver argsResolver, AvailabilityService availabilityService)
{
    public void Run(string[] args)
    {
        argsResolver.ResolveArgs(args, out var hotels, out var bookings);

        string input;

        Console.WriteLine($"Welcome to the BookMe.\nCheck rooms availability with Availability(<hotelId>, <dateRange>, <roomType>)");

        while ((input = Console.ReadLine()!) != string.Empty)
        {
            if (input.StartsWith("Availability"))
                Console.WriteLine(availabilityService.CheckAvailability(input, hotels, bookings));
            else
                Console.WriteLine("Wrong command. Try again..");
        }
    }
}
