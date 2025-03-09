using Unix_Ref.Collector.Exception;
using Unix_Ref.Uno;

namespace Unix_Ref.Collector;

static class ObjSize
{
    public static readonly uint CardSize = 16;
    public static readonly uint DeckSize = 8;
    public static readonly uint PlayerSize = 16;
    public static readonly uint GameSize = 24;
}
public class ManagedObject
{
    public static uint NextId;
    
    public uint Id { get; }
    public int Generation { get; set; }
    public Object? Obj { get; set; }
    public uint Size { get; set; }
    public uint Count { get; set; }

    public ManagedObject(Object? obj)
    {
        if (obj is not (null or Card or Deck or Game or Player))
            throw new InvalidTypeManagedObjectException("Cannot create managed object from a non Uno class type.", obj.GetType());
        Obj = obj;
        Id = NextId++;
        Generation = 0;
        Count = 1;
        Size = ComputeSize(obj);
    }
    
    private uint ComputeSize(Object? obj)
    {
        return obj switch
        {
            Game game => ObjSize.GameSize + (uint)game.Players.Count * 8,
            Player player => ObjSize.PlayerSize + (uint)player.Cards.Count * 8 + (uint)player.GetName().Length * 2,
            Deck deck => ObjSize.DeckSize + (uint)deck.Cards.Count * 8,
            _ => obj is Card ? ObjSize.CardSize : 0
        };
    }
}