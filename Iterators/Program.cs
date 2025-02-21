// See https://aka.ms/new-console-template for more information

using Iterators.Fundamentals;
using Iterators.Fundamentals.Uno;

Console.WriteLine("Hello, World!");
Deck deck = new Deck();
int i = 0;
foreach (Card c in deck)
{
    i++;
    Console.WriteLine(c.ToString());
}
Console.WriteLine(i);
i = 0;
foreach (Card c in deck)
{
    i++;
    Console.WriteLine(c.ToString());
}
Console.WriteLine(i);