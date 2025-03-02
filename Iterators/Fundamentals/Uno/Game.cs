namespace Iterators.Fundamentals.Uno;

public class Game
{
    public Deck deck;
    List<Player> players;
    public Card current;
    public int nbOfPlus;

    public Game()
    {
        deck = new Deck();
        players = new List<Player>();
        nbOfPlus = 0;
        deck.Shuffle();
    }
    
    public void Eat(int n)
    {
        deck.UpdateDeck(n);
    }
    
    public void AddPlayer(string playerName)
    {
        players.Add(new Player(this, playerName));
        Eat(7);
    }

    private bool GameOver()
    {
        foreach (Player player in players)
        {
            if (player.Won())
                return true;
        }

        return deck.IsEmpty();
    }

    private int FindBefore(Player who)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == who)
                return i == 0 ? players.Count - 1 : i - 1;
        }
        return -1;
    }

    private void RightRotate(int nb)
    {
        int n = players.Count;
        for (int i = 0; i < nb; i++) 
        {
            Player p = players[n - 1];
            for (int j = n - 2; j >= 0; j--) 
                players[j + 1] = players[j];
            players[0] = p;
        }
    }

    private void LeftRotate(int nb)
    {
        int n = players.Count;
        for (int i = 0; i < nb; i++) 
        {
            Player p = players[0];
            for (int j = 0; j < n - 1; j++) 
                players[j] = players[j + 1];
            players[n - 1] = p;
        }
    }

    private void Reverse()
    {
        for (int i = 0; i < players.Count / 2; i++)
        {
            (players[i], players[players.Count - i - 1]) = (players[players.Count - i - 1], players[i]);
        }
    }

    private void ReverseList(Player who)
    {
        int first = FindBefore(who);
        int n = players.Count;
        if (first > n / 2)
            RightRotate(n - first);
        else
            LeftRotate(first);
        Reverse();
    }
    
    public void Play()
    {
        int n;
        Card? c = deck.GetFirst();
        if (c is null)
            return;
        current = (Card)c;
        Player? p = null;
        while (!GameOver())
        {
            foreach (Player player in players)
            {
                Console.Write("current card = " + current + "  \t");
                n = player.Play();
                Console.Write(player.GetName() + "'s turn, he has : " + player.GetCards());
                Console.WriteLine(player + " he drew " + n + " cards and played " + current + "\n");
                if (n == -400)
                    throw new ArgumentException(); //ahah -400 cards, funny right
                if (n == -1)
                {
                    if (current.Rank == Rank.Invert)
                    {
                        p = player;
                        current.Rank = Rank.Used;
                        break;
                    }
                    current.Rank = Rank.Used;
                }
                else if (n == -2)
                    nbOfPlus += current.Rank == Rank.Plus2 ? 2 : 4;
                else if (n >= 0)
                {
                    Eat(n);
                    if (current.Rank == Rank.Plus2 || current.Rank == Rank.Plus4)
                        nbOfPlus = current.Rank == Rank.Plus2 ? 2 : 4;
                    else
                        nbOfPlus = 0;
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
        foreach (Player player in players)
        {
            Console.WriteLine(player);
            if (player.Won())
                finished = true;
        }
        if (deck.IsEmpty() && !finished)
            Console.WriteLine("This game did not end.");
    }
}