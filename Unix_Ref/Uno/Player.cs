namespace Unix_Ref.Uno;

public class Player
{
    protected string Name;
    public readonly List<Card> Cards;
    protected Game Game;

    public Player(Game game, string name)
    {
        Name = name;
        Cards = new List<Card>();
        Game = game;
        int i = 0;
        foreach (Card card in game.Deck)
        {
            Cards.Add(card);
            i++;
            if (i == 7)
                break;
        }
    }

    public static bool operator==(Player player, Player other)
    {
        return player.Name == other.Name && player.Game == other.Game;
    }

    public static bool operator !=(Player player, Player other)
    {
        return !(player == other);
    }

    public bool Won()
    {
        return Cards.Count == 0;
    }

    protected Card? CanPlayPlus()
    {
        Rank toFind = Game.Current.Rank == Rank.Plus2 ? Rank.Plus2 : Rank.Plus4;
        foreach (Card card in Cards)
        {
            if (card.Rank == toFind)
                return card;
        }

        return null;
    }

    protected virtual bool Draw(int n)
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
    
    protected bool CanPlay()
    {
        foreach (Card card in Cards)
        {
            if (card.Color == Game.Current.Color || card.Rank == Game.Current.Rank || card.Color == Color.Special)
                return true;
        }
        return false;
    }
    
    protected virtual int PlayCard()
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

    protected virtual int PlusHandler()
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

    protected Color MaxCol()
    {
        int r = 0;
        int g = 0;
        int y = 0;
        int b = 0;
        foreach (Card card in Cards)
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
        switch (Game.Current.Rank)
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
            return Name + " won the game !";
        return Name + " has " + Cards.Count + " cards in his hand.";
    }

    public string GetCards()
    {
        string cards = "\n";
        foreach (Card card in Cards)
        {
            cards += "\t- " + card + "\n";
        }
        return cards;
    }

    public string GetName()
    {
        return Name;
    }
}