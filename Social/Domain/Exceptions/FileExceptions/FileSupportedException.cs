
namespace Domain.Exceptions;

public class FileSupportedException : Exception
{
    public FileSupportedException() : base("Upload a valid video file.") { }
    public FileSupportedException(string message) : base(message) { }

}
