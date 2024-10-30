namespace BookMe.Services;

public interface IFileReader
{
    public bool IsValidPath(string path);
    public T? ReadFile<T>(string path);
}