namespace Iterators.Fundamentals.Uno;

public enum Color { Red, Green, Yellow, Blue, Special }
public enum Rank { Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Plus2, Invert, Block, Joker, Plus4, Used }

public struct Card : IEquatable<Card>
{
    public Color Color { get; set; }
    public Rank Rank { get; set; }

    public Card(Color color, Rank rank)
    {
        Color = color;
        Rank = rank;
    }

    public override string ToString() => $"{Rank} of {Color}";

    public bool Equals(Card other)
    {
        return Color == other.Color && Rank == other.Rank;
    }

    public override bool Equals(object? obj)
    {
        return obj is Card other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Color, (int)Rank);
    }
}