namespace Iterators.Fundamentals.Uno;

public class Game
{
    public Deck deck;
    List<Player> players;
    public Card current;

    public Game()
    {
        deck = new Deck();
        players = new List<Player>();
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
    
    public void Play()
    {
        int n;
        current = deck.GetFirst();
        while (!GameOver())
        {
            foreach (Player player in players)
            {
                n = player.Play();
                if (n != -1)
                    Eat(n);
            }
        }
    }
}