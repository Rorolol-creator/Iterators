namespace Unix_Ref.Collector;

public class NullManagedObjectException : Exception
{
    public NullManagedObjectException(string message) : base(message) {}
}