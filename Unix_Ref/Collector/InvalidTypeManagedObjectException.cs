namespace Unix_Ref.Collector;

public class InvalidTypeManagedObjectException : Exception
{
    public Type Type { get; }
    public InvalidTypeManagedObjectException(string message, Type type) : base(message)
    {
        Type = type;
    }
}