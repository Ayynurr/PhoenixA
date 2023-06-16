namespace Domain.Exceptions;

public class FileSizeException : Exception
{
    public FileSizeException() : base("File size must be max 2kb ") { }
    public FileSizeException(string message) : base(message) { }
}
