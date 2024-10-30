namespace BookMe.Services;

public class DateParser
{
    private const string DateSeparator = "-";
    private const string DateFormat = "yyyyMMdd";
    public void ParseDateRange(string dateRange, out DateTime arrival, out DateTime departure)
    {
        try
        {
            arrival = DateTime.MinValue;
            departure = DateTime.MinValue;

            var dateParts = dateRange.Split(DateSeparator);

            arrival = DateTime.ParseExact(dateParts[0], DateFormat, null);

            if (dateParts.Length > 1)
                departure = DateTime.ParseExact(dateParts[1], DateFormat, null);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Wrong date format. ({DateFormat} or {DateFormat}-{DateFormat})");
            throw;
        }
    }

    public DateTime Parse(string date)
    {
        try
        {
            return DateTime.ParseExact(date, DateFormat, null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}