namespace Unix_Ref.Uno;

public class Player
{
    private string _name;
    private List<Card> _cards;
    private Game _game;

    public Player(Game game, string name)
    {
        _name = name;
        _cards = new List<Card>();
        _game = game;
        int i = 0;
        foreach (Card card in game.Deck)
        {
            _cards.Add(card);
            i++;
            if (i == 7)
                break;
        }
    }

    public static bool operator==(Player player, Player other)
    {
        return player._name == other._name && player._game == other._game;
    }

    public static bool operator !=(Player player, Player other)
    {
        return !(player == other);
    }

    public bool Won()
    {
        return _cards.Count == 0;
    }

    private Card? CanPlayPlus()
    {
        Rank toFind = _game.Current.Rank == Rank.Plus2 ? Rank.Plus2 : Rank.Plus4;
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
        foreach (Card card in _game.Deck)
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
            if (card.Color == _game.Current.Color || card.Rank == _game.Current.Rank || card.Color == Color.Special)
                return true;
        }
        return false;
    }
    
    private int PlayCard()
    {
        int i = 0;
        if (!CanPlay())
        {
            foreach (Card card in _game.Deck)
            {
                _cards.Add(card);
                i++;
                if (card.Color == _game.Current.Color || card.Rank == _game.Current.Rank || card.Color == Color.Special)
                    break;
            }
        }
        bool played = false;
        foreach (Card card in _cards)
        {
            if (card.Color == _game.Current.Color)
            {
                _game.Current = card;
                played = true;
                break;
            }
        }

        if (played)
        {
            _cards.Remove(_game.Current);
            return i;
        }
        
        foreach (Card card in _cards)
        {
            if (card.Rank == _game.Current.Rank)
            {
                _game.Current = card;
                played = true;
                break;
            }
        }

        if (played)
        {
            _cards.Remove(_game.Current);
            return i;
        }

        Card c = _cards[0];
        foreach (Card card in _cards)
        {
            if (card.Color == Color.Special)
            {
                _game.Current = card;
                c = card;
                played = true;
                _game.Current.Color = MaxCol();
                break;
            }
        }
        if (played)
        {
            _cards.Remove(c);
            return i;
        }
        return -1;
    }

    private int PlusHandler()
    {
        Card? card = CanPlayPlus();
        if (card is not null)
        {
            _game.Current = card;
            _cards.Remove(card);
            return -1;
        }
        if (!Draw(_game.NbOfPlus))
            return -400;
        _game.Eat(_game.NbOfPlus);
        return _game.NbOfPlus;
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
        switch (_game.Current.Rank)
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
            return _name + " won the game !";
        return _name + " has " + _cards.Count + " cards in his hand.";
    }

    public string GetCards()
    {
        string cards = "\n";
        foreach (Card card in _cards)
        {
            cards += "\t- " + card + "\n";
        }
        return cards;
    }

    public string GetName()
    {
        return _name;
    }
}