using Unix_Ref.Uno;

namespace Unix_Ref.Collector;

public class ManagedObject
{
    public static uint NextId;
    
    public uint Id { get; }
    public int Generation { get; set; }
    public Object? Obj { get; set; }
    public int Size { get; set; }
    public bool IsAlive { get; set; }

    public ManagedObject(Object? obj)
    {
        if (obj is null)
            throw new NullManagedObjectException("Cannot create managed object from a null object.");
        if (obj is not (Card or Deck or Game or Player))
            throw new InvalidTypeManagedObjectException("Cannot create managed object from a non Uno class type.", obj.GetType());
        Obj = obj;
        Id = NextId++;
        Generation = 0;
        IsAlive = true;
        Size = ComputeSize(obj);
    }

    private int ComputeSize(Object? obj)
    {
        
    }
}