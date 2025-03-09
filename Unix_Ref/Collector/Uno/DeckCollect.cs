using Unix_Ref.Collector.Exception;
using Unix_Ref.Uno;

namespace Unix_Ref.Collector.Uno;

public class DeckCollect : Deck
{
    public readonly uint Id;
    public DeckCollect()
    {
        var obj = new ManagedObject(this);
        Id = obj.Id;
        var res = GarbageCollector.Instance().Heap.Add(obj);
        if (!res.Item1)
            throw new NotEnoughMemoryException(res.Item2);
        Console.WriteLine(res.Item2);
    }

    public override void UpdateDeck(int drawnCards)
    {
        while (drawnCards > 0)
        {
            MemoryHeap heap = GarbageCollector.Instance().Heap;
            foreach (var obj in heap)
            {
                if (obj.Id == ((CardCollect)Cards[0]).Id)
                {
                    obj.Count -= 1;
                }
            }
            Cards.RemoveAt(0);
            drawnCards--;
        }
    }

    public override CardCollect? GetFirst()
    {
        int i = 0;
        while (i < Cards.Count && (Cards[i].Rank == Rank.Plus2 || Cards[i].Color == Color.Special || Cards[i].Rank == Rank.Invert || Cards[i].Rank == Rank.Block))
            i++;
        if (i == Cards.Count)
            return null;
        CardCollect c = (CardCollect)Cards[i];
        MemoryHeap heap = GarbageCollector.Instance().Heap;
        foreach (var obj in heap)
        {
            if (obj.Id == c.Id)
            {
                obj.Count -= 1;
            }
        }
        Cards.RemoveAt(i);
        return c;
    }
    
    public void Destroy()
    {
        MemoryHeap heap = GarbageCollector.Instance().Heap;
        foreach (var card in Cards)
        {
            heap.ChangeCount(((CardCollect)card).Id, -1);
        }
    }
}