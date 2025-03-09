using System.Collections;

namespace Unix_Ref.Uno;

public class Deck : IEnumerable<Card>
{
    public readonly List<Card> Cards = new();

    public Deck()
    {
        foreach (Color c in Enum.GetValues(typeof(Color)))
        {
            if (c == Color.Special)
            {
                for (int i = 0; i < 4; i++)
                {
                    Cards.Add(new Card(Color.Special, Rank.Joker));
                    Cards.Add(new Card(Color.Special, Rank.Plus4));
                }
                continue;
            }
            foreach (Rank r in Enum.GetValues(typeof(Rank)))
            {
                if (r == Rank.Joker)
                    break;
                if (r != Rank.Zero)
                    Cards.Add(new Card(c, r));
                Cards.Add(new Card(c, r));
            }
        }
    }

    public virtual void UpdateDeck(int drawnCards)
    {
        while (drawnCards > 0)
        {
            Cards.RemoveAt(0);
            drawnCards--;
        }
    }

    public IEnumerator<Card> GetEnumerator()
    {
        foreach (Card c in Cards)
        {
            yield return c;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public virtual Card? GetFirst()
    {
        int i = 0;
        while (i < Cards.Count && (Cards[i].Rank == Rank.Plus2 || Cards[i].Color == Color.Special || Cards[i].Rank == Rank.Invert || Cards[i].Rank == Rank.Block))
            i++;
        if (i == Cards.Count)
            return null;
        Card c = Cards[i];
        Cards.RemoveAt(i);
        return c;
    }
    
    public bool IsEmpty() => Cards.Count == 0;

    public void Shuffle()
    {
        Random rd = new Random(42);
        for (int i = 0; i < Cards.Count; i++)
        {
            int otherPos = rd.Next(Cards.Count);
            (Cards[i], Cards[otherPos]) = (Cards[otherPos], Cards[i]);
        }
    }
    
    public int NbofCards => Cards.Count;
}