namespace BookMe.Tests;

using FluentAssertions;
using Services;

public class DateParserTests
{
    [Theory]
    [MemberData(nameof(GetValidParseDateRangeData))]
    public void ParseDateRange_ForValidData_ReturnsCorrectOutput(string dateRange, DateTime expectedArrival,
        DateTime expectedDeparture)
    {
        var parser = new DateParser();

        parser.ParseDateRange(dateRange, out var arrival, out var departure);

        arrival.Should().Be(expectedArrival);
        departure.Should().Be(expectedDeparture);
    }

    [Theory]
    [MemberData(nameof(GetInvalidParseDateRangeData))]
    public void ParseDateRange_ForInvalidData_ThrowsException(string dateRange)
    {
        var parser = new DateParser();

        var action = () => parser.ParseDateRange(dateRange, out _, out _);

        action.Invoking(a => a.Invoke()).Should().Throw<Exception>();
    }

    [Theory]
    [MemberData(nameof(GetValidParseData))]
    public void Parse_ForValidDate_ReturnsParsedDateTime(string date, DateTime expectedResult)
    {
        var parser = new DateParser();

        var result = parser.Parse(date);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("1132")]
    [InlineData("111506a1")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Aa")]
    [InlineData(null)]
    [InlineData("2024091")]
    [InlineData("20240901-20240902")]
    [InlineData("20240933")]
    public void Parse_ForInvalidDate_ThrowsException(string date)
    {
        var parser = new DateParser();

        var action = () => parser.Parse(date);

        action.Invoking(a => a.Invoke()).Should().Throw<Exception>();
    }

    public static IEnumerable<object[]> GetValidParseDateRangeData()
    {
        yield return [ "20240901", new DateTime(2024, 9, 1), DateTime.MinValue ];
        yield return [ "20230531", new DateTime(2023, 5, 31), DateTime.MinValue ];
        yield return [ "20240901-20241010", new DateTime(2024, 9, 1), new DateTime(2024, 10, 10) ];
    }

    public static IEnumerable<object[]> GetInvalidParseDateRangeData()
    {
        yield return [ "123156516516"];
        yield return [ "20230531-1"];
        yield return [ "aasd"];
        yield return [ "---"];
        yield return [ "-"];
        yield return [ "-20240901-20241010"];
        yield return [ "",];
    }

    public static IEnumerable<object[]> GetValidParseData()
    {
        yield return [ "20240901", new DateTime(2024, 9, 1) ];
        yield return [ "20230531", new DateTime(2023, 5, 31) ];
        yield return [ "20240901", new DateTime(2024, 9, 1) ];
        yield return [ "19561101", new DateTime(1956, 11, 01) ];
    }
}