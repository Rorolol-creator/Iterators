namespace Unix_Ref.Collector;

public class GarbageCollector
{
    private static GarbageCollector? _instance;
    
    public static GarbageCollector Instance(int maxSize) => _instance ??= new GarbageCollector(new MemoryHeap(maxSize));

    public MemoryHeap Heap { get; }
    public GarbageCollector(MemoryHeap heap)
    {
        Heap = heap;
    }
}