using Unix_Ref.Collector.Exception;
using Unix_Ref.Uno;

namespace Unix_Ref.Collector.Uno;

public class CardCollect : Card
{
    public readonly uint Id;
    public CardCollect(Color color, Rank rank) : base(color, rank)
    {
        var obj = new ManagedObject(this);
        Id = obj.Id;
        var res = GarbageCollector.Instance().Heap.Add(obj);
        if (!res.Item1)
            throw new NotEnoughMemoryException(res.Item2);
        Console.WriteLine(res.Item2);
    }
}