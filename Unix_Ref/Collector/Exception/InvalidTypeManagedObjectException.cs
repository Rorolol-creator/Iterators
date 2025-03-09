namespace Unix_Ref.Collector.Exception;

public class InvalidTypeManagedObjectException : System.Exception
{
    public Type Type { get; }
    public InvalidTypeManagedObjectException(string message, Type type) : base(message)
    {
        Type = type;
    }
}