namespace Iterators.Fundamentals.Uno;

public class Player
{
    private string name;
    private List<Card> _cards;
    private Game _game;

    public Player(Game game, string name)
    {
        this.name = name;
        _cards = new List<Card>();
        _game = game;
        int i = 0;
        foreach (Card card in game.deck)
        {
            _cards.Add(card);
            i++;
            if (i == 7)
                break;
        }
    }

    public static bool operator+(Player player, Player other)
    {
        return player.name == other.name && player._game == other._game;
    }
    
    public bool Won()
    {
        return _cards.Count == 0;
    }

    private Card? CanPlayPlus()
    {
        Rank toFind = _game.current.Rank == Rank.Plus2 ? Rank.Plus2 : Rank.Plus4;
        foreach (Card card in _cards)
        {
            if (card.Rank == toFind)
                return card;
        }

        return null;
    }

    private bool Draw(int n)
    {
        int i = 0;
        foreach (Card card in _game.deck)
        {
            _cards.Add(card);
            i++;
            if (i == n)
                break;
        }
        return i == n;
    }
    
    private bool CanPlay()
    {
        foreach (Card card in _cards)
        {
            if (card.Color == _game.current.Color || card.Rank == _game.current.Rank || card.Color == Color.Special)
                return true;
        }
        return false;
    }
    
    private int PlayCard()
    {
        int i = 0;
        if (!CanPlay())
        {
            foreach (Card card in _game.deck)
            {
                _cards.Add(card);
                i++;
                if (card.Color == _game.current.Color || card.Rank == _game.current.Rank || card.Color == Color.Special)
                    break;
            }
        }
        bool played = false;
        foreach (Card card in _cards)
        {
            if (card.Color == _game.current.Color)
            {
                _game.current = card;
                played = true;
                break;
            }
        }

        if (played)
        {
            _cards.Remove(_game.current);
            return i;
        }
        
        foreach (Card card in _cards)
        {
            if (card.Rank == _game.current.Rank)
            {
                _game.current = card;
                played = true;
                break;
            }
        }

        if (played)
        {
            _cards.Remove(_game.current);
            return i;
        }
        
        foreach (Card card in _cards)
        {
            if (card.Color == Color.Special)
            {
                _game.current = card;
                played = true;
                _game.current.Color = MaxCol();
                break;
            }
        }
        if (played)
        {
            _cards.Remove(_game.current);
            return i;
        }
        return -1;
    }

    private int PlusHandler()
    {
        Card? card = CanPlayPlus();
        if (card is not null)
        {
            _game.current = (Card)card;
            _cards.Remove((Card)card);
            return -1;
        }
        if (!Draw(_game.nbOfPlus))
            return -400;
        _game.Eat(_game.nbOfPlus);
        return _game.nbOfPlus;
    }

    private Color MaxCol()
    {
        int r = 0;
        int g = 0;
        int y = 0;
        int b = 0;
        foreach (Card card in _cards)
        {
            switch (card.Color)
            {
                case Color.Red:
                    r++;
                    break;
                case Color.Green:
                    g++;
                    break;
                case Color.Yellow:
                    y++;
                    break;
                case Color.Blue:
                    b++;
                    break;
            }
        }

        if (r >= g && r >= y && r >= b)
            return Color.Red;
        if (g >= r && g >= y && g >= b)
            return Color.Green;
        if (y >= r && y >= g && y >= b)
            return Color.Yellow;
        return Color.Blue;
    }
    
    public int Play()
    {
        switch (_game.current.Rank)
        {
            case Rank.Plus2:
            case Rank.Plus4:
                int ret = PlusHandler();
                if (ret == -1)
                    return -2;
                break;
            case Rank.Invert:
                return -1;
            case Rank.Block:
                return -1;
        }
        int played = PlayCard();
        if (played == -1)
            return -400;
        return played;
    }
    
    public override string ToString()
    {
        if (Won())
            return name + " won the game !";
        return name + " has " + _cards.Count + " cards in his hand.";
    }
}