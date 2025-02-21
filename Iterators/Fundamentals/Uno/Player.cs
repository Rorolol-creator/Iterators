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

    public bool Won()
    {
        return _cards.Count == 0;
    }

    private bool CanPlayPlus()
    {
        Rank toFind = _game.current.Rank == Rank.Plus2 ? Rank.Plus2 : Rank.Plus4;
        foreach (Card card in _cards)
        {
            if (card.Rank == toFind)
                return true;
        }

        return false;
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
        foreach (Card card in _game.deck)
        {
            if (CanPlay())
                break;
            _cards.Add(card);
            i++;
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
                break;
            }
        }
        if (played)
            return i;
        return -1;
    }
    public int Play()
    {
        switch (_game.current.Rank)
        {
            case Rank.Plus2:
                if (!Draw(2))
                    return -1;
                _game.Eat(2);
                break;
            case Rank.Invert:
                _game.current.Rank = Rank.Used;
                return 0;
            case Rank.Block:
                _game.current.Rank = Rank.Used;
                return 0;
            case Rank.Plus4:
                if (!Draw(4))
                    return -1;
                _game.Eat(4);
                break;
        }
        int played = PlayCard();
        if (played == -1)
            return -1;
        return played;
    }
    
    public override string ToString()
    {
        if (Won())
            return name + " won the game !";
        return name + " has " + _cards.Count + " cards in his hand.";
    }
}