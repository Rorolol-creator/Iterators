namespace Unix_Ref.Collector;

public class MemoryHeap: IEnumerable<ManagedObject>
{
    private List<ManagedObject> _managedObjects = new();
    
    public int CurrentSize { get; private set; }
    
    public int MaxMemorySize { get; }

    public MemoryHeap(int maxMemorySize)
    {
        MaxMemorySize = maxMemorySize;
        CurrentSize = 0;
    }
    
    
}