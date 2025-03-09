namespace Unix_Ref.Collector.Exception;

public class NotEnoughMemoryException : System.Exception
{
    public NotEnoughMemoryException(string message) : base(message) {}
}