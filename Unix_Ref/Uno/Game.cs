namespace Unix_Ref.Uno;

public class Game
{
    public Deck Deck { get; }
    public readonly List<Player> Players;
    private Card? _current;
    public virtual Card? Current { get => _current; set => _current = value; }
    public int NbOfPlus;

    public Game()
    {
        Deck = new Deck();
        Players = new List<Player>();
        NbOfPlus = 0;
        Deck.Shuffle();
    }
    
    public void Eat(int n)
    {
        Deck.UpdateDeck(n);
    }
    
    public virtual void AddPlayer(string playerName)
    {
        Players.Add(new Player(this, playerName));
        Eat(7);
    }

    protected bool GameOver()
    {
        foreach (Player player in Players)
        {
            if (player.Won())
                return true;
        }

        return Deck.IsEmpty();
    }

    private int FindBefore(Player who)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i] == who)
                return i == 0 ? Players.Count - 1 : i - 1;
        }
        return -1;
    }

    private void RightRotate(int nb)
    {
        int n = Players.Count;
        for (int i = 0; i < nb; i++) 
        {
            Player p = Players[n - 1];
            for (int j = n - 2; j >= 0; j--) 
                Players[j + 1] = Players[j];
            Players[0] = p;
        }
    }

    private void LeftRotate(int nb)
    {
        int n = Players.Count;
        for (int i = 0; i < nb; i++) 
        {
            Player p = Players[0];
            for (int j = 0; j < n - 1; j++) 
                Players[j] = Players[j + 1];
            Players[n - 1] = p;
        }
    }

    private void Reverse()
    {
        for (int i = 0; i < Players.Count / 2; i++)
        {
            (Players[i], Players[Players.Count - i - 1]) = (Players[Players.Count - i - 1], Players[i]);
        }
    }

    protected void ReverseList(Player who)
    {
        int first = FindBefore(who);
        int n = Players.Count;
        if (first > n / 2)
            RightRotate(n - first);
        else
            LeftRotate(first);
        Reverse();
    }
    
    public virtual void Play()
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
}