using Unix_Ref.Collector.Exception;
using Unix_Ref.Uno;

namespace Unix_Ref.Collector.Uno;

public class PlayerCollect : Player
{
    public readonly uint Id;
    public PlayerCollect(GameCollect gameCollect, string name) : base(gameCollect, name)
    {
        var obj = new ManagedObject(this);
        Id = obj.Id;
        var res = GarbageCollector.Instance().Heap.Add(obj);
        if (!res.Item1)
            throw new NotEnoughMemoryException(res.Item2);
        Console.WriteLine(res.Item2);
    }

    protected override bool Draw(int n) // TODO
    {
        int i = 0;
        foreach (Card card in Game.Deck)
        {
            Cards.Add(card);
            i++;
            if (i == n)
                break;
        }
        return i == n;
    }
    
    protected override int PlayCard() // TODO
    {
        int i = 0;
        if (!CanPlay())
        {
            foreach (Card card in Game.Deck)
            {
                Cards.Add(card);
                i++;
                if (card.Color == Game.Current.Color || card.Rank == Game.Current.Rank || card.Color == Color.Special)
                    break;
            }
        }
        bool played = false;
        foreach (Card card in Cards)
        {
            if (card.Color == Game.Current.Color)
            {
                Game.Current = card;
                played = true;
                break;
            }
        }

        if (played)
        {
            Cards.Remove(Game.Current);
            return i;
        }
        
        foreach (Card card in Cards)
        {
            if (card.Rank == Game.Current.Rank)
            {
                Game.Current = card;
                played = true;
                break;
            }
        }

        if (played)
        {
            Cards.Remove(Game.Current);
            return i;
        }

        Card c = Cards[0];
        foreach (Card card in Cards)
        {
            if (card.Color == Color.Special)
            {
                Game.Current = card;
                c = card;
                played = true;
                Game.Current.Color = MaxCol();
                break;
            }
        }
        if (played)
        {
            Cards.Remove(c);
            return i;
        }
        return -1;
    }

    protected override int PlusHandler() // TODO
    {
        Card? card = CanPlayPlus();
        if (card is not null)
        {
            Game.Current = card;
            Cards.Remove(card);
            return -1;
        }
        if (!Draw(Game.NbOfPlus))
            return -400;
        Game.Eat(Game.NbOfPlus);
        return Game.NbOfPlus;
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