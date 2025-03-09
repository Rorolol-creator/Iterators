using Unix_Ref.Collector.Exception;
using Unix_Ref.Uno;

namespace Unix_Ref.Collector.Uno;

public class GameCollect : Game
{
    public readonly uint Id;
    public override Card? Current
    {
        get => base.Current;
        set
        {
            if (Current is not null)
                foreach (var obj in GarbageCollector.Instance().Heap)
                {
                    if (obj.Id == ((CardCollect)Current).Id)
                    {
                        obj.Count -= 1;
                    }
                }
            base.Current = value;
        }
    }

    public GameCollect()
    {
        var obj = new ManagedObject(this);
        Id = obj.Id;
        var res = GarbageCollector.Instance().Heap.Add(obj);
        if (!res.Item1)
            throw new NotEnoughMemoryException(res.Item2);
        Console.WriteLine(res.Item2);
    }
    public override void AddPlayer(string playerName)
    {
        PlayerCollect player = new PlayerCollect(this, playerName);
        GarbageCollector.Instance().Heap.Add(new ManagedObject(player));
        Players.Add(player);
        Eat(7);
    }
    
    public override void Play() // TODO
    {
        int n;
        Card? c = Deck.GetFirst();
        if (c is null)
            return;
        Current = c;
        Player? p = null;
        while (!GameOver())
        {
            foreach (Player player in Players)
            {
                Console.Write("current card = " + Current + "  \t");
                n = player.Play();
                Console.Write(player.GetName() + "'s turn, he has : " + player.GetCards());
                Console.WriteLine(player + " he drew " + n + " cards and played " + Current + "\n");
                if (n == -400)
                {
                    Eat(Deck.NbofCards);
                    break;
                }
                if (n == -1)
                {
                    if (Current.Rank == Rank.Invert)
                    {
                        p = player;
                        Current.Rank = Rank.Used;
                        break;
                    }
                    Current.Rank = Rank.Used;
                }
                else if (n == -2)
                    NbOfPlus += Current.Rank == Rank.Plus2 ? 2 : 4;
                else if (n >= 0)
                {
                    Eat(n);
                    if (Current.Rank == Rank.Plus2 || Current.Rank == Rank.Plus4)
                        NbOfPlus = Current.Rank == Rank.Plus2 ? 2 : 4;
                    else
                        NbOfPlus = 0;
                }
                if (GameOver())
                    break;
            }

            if (p is not null)
            {
                ReverseList(p);
                p = null;
            }
        }

        bool finished = false;
        foreach (Player player in Players)
        {
            Console.WriteLine(player);
            if (player.Won())
                finished = true;
        }
        if (Deck.IsEmpty() && !finished)
            Console.WriteLine("This game did not end.");
    }
    
    public void Destroy()
    {
        MemoryHeap heap = GarbageCollector.Instance().Heap;
        foreach (var player in Players)
        {
            heap.ChangeCount(((PlayerCollect)player).Id, -1);
        }

        if (Current is not null)
        {
            foreach (var obj in heap)
            {
                if (obj.Id == ((CardCollect)Current).Id || obj.Id == ((DeckCollect)Deck).Id)
                {
                    obj.Count -= 1;
                }
            }
        }
        else
        {
            heap.ChangeCount(((DeckCollect)Deck).Id, -1);
        }
    }
}