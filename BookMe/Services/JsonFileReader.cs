namespace BookMe.Services;

using System.Text.Json;

public class JsonFileReader : IFileReader
{
    public bool IsValidPath(string path)
    {
        try
        {
            if (path.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                return false;

            var fullPath = Path.GetFullPath(path);

            if (fullPath.Length > 259)
                return false;

            if (fullPath.Length > Environment.SystemPageSize * 16)
                return false;

            return File.Exists(fullPath);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error validating path: {e.Message}");
            return false;
        }
    }

    public T? ReadFile<T>(string path)
    {
        try
        {
            using var reader = new StreamReader(path);

            var jsonString = reader.ReadToEnd();
            return JsonSerializer.Deserialize<T>(jsonString);
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}\\n {e.StackTrace}");
            return default;
        }
    }
}