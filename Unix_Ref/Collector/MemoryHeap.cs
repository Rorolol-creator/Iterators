using System.Collections;
using Unix_Ref.Collector.Exception;

namespace Unix_Ref.Collector;

public class MemoryHeap: IEnumerable<ManagedObject>
{
    private List<ManagedObject> _managedObjects = new();
    
    public uint CurrentSize { get; private set; }
    
    public uint MaxMemorySize { get; }

    public MemoryHeap(uint maxMemorySize)
    {
        MaxMemorySize = maxMemorySize;
        CurrentSize = 0;
    }

    public (bool, String) Add(ManagedObject managedObject)
    {
        if (managedObject.Obj == null)
            throw new NullManagedObjectException("Cannot add a null object to the heap");
        if (managedObject.Size > MaxMemorySize - CurrentSize)
            return (false, $"Not enough memory to add object of type {managedObject.Obj.GetType()}: " +
                           $"\tMax Size -> {MaxMemorySize}" +
                           $"\tCurrent Size -> {CurrentSize}" +
                           $"\tNeeded Size -> {managedObject.Size}");
        CurrentSize += managedObject.Size;
        _managedObjects.Add(managedObject);
        return (true, $"Added object of type {managedObject.Obj.GetType()} to the heap");
    }

    public void ChangeCount(uint id, int diffCount)
    {
        foreach (var obj in _managedObjects)
        {
            if (obj.Id == id)
            {
                if (obj.Count == 0 && diffCount < 0)
                    throw new InvalidCountManagedObjectException("Cannot have a count < 0");
                if (diffCount < 0)
                    obj.Count -= (uint)(diffCount * -1);
                else
                    obj.Count += (uint)diffCount;
            }
        }
    }
    
    public IEnumerator<ManagedObject> GetEnumerator()
    {
        foreach (ManagedObject obj in _managedObjects)
        {
            yield return obj;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}