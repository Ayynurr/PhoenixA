namespace Domain.Exceptions;

public class FileTypeException : Exception
{
    public FileTypeException() : base("File type must be image") { }

    public FileTypeException(string message) : base(message) { }

}
