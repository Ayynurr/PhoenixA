namespace Domain.Exceptions;

public class NotfoundException : Exception
{
    public NotfoundException():base("Not Found Exception") { }   
    public NotfoundException(string message) : base(message) { }
   
}
