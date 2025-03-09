using System.Collections;

namespace Unix_Ref.Uno;

public class Deck : IEnumerable<Card>
{
    private List<Card> _cards = new List<Card>();

    public Deck()
    {
        foreach (Color c in Enum.GetValues(typeof(Color)))
        {
            if (c == Color.Special)
            {
                for (int i = 0; i < 4; i++)
                {
                    _cards.Add(new Card(Color.Special, Rank.Joker));
                    _cards.Add(new Card(Color.Special, Rank.Plus4));
                }
                continue;
            }
            foreach (Rank r in Enum.GetValues(typeof(Rank)))
            {
                if (r == Rank.Joker)
                    break;
                if (r != Rank.Zero)
                    _cards.Add(new Card(c, r));
                _cards.Add(new Card(c, r));
            }
        }
    }

    public void UpdateDeck(int drawnCards)
    {
        while (drawnCards > 0)
        {
            _cards.RemoveAt(0);
            drawnCards--;
        }
    }

    public IEnumerator<Card> GetEnumerator()
    {
        foreach (Card c in _cards)
        {
            yield return c;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Card? GetFirst()
    {
        int i = 0;
        while (i < _cards.Count && (_cards[i].Rank == Rank.Plus2 || _cards[i].Color == Color.Special || _cards[i].Rank == Rank.Invert || _cards[i].Rank == Rank.Block))
            i++;
        if (i == _cards.Count)
            return null;
        Card c = _cards[i];
        _cards.RemoveAt(i);
        return c;
    }
    
    public bool IsEmpty() => _cards.Count == 0;

    public void Shuffle()
    {
        Random rd = new Random(42);
        for (int i = 0; i < _cards.Count; i++)
        {
            int otherPos = rd.Next(_cards.Count);
            (_cards[i], _cards[otherPos]) = (_cards[otherPos], _cards[i]);
        }
    }
    
    public int NbofCards => _cards.Count;
}