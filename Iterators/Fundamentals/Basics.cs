namespace Iterators.Fundamentals;

public class Basics
{
    public static IEnumerable<int> GetNumbers(int n)
    {
        for (int i = 1; i <= n; i++)
        {
            yield return i;
        }
    }
    
    public static IEnumerable<int> GetEvenNumbers(int start, int end)
    {
        for (int i = start; i <= end; i++)
        {
            if (i % 2 == 0)
            {
                yield return i;
            }
        }
    }
    
    public static IEnumerable<int> GetFibonacciSequence()
    {
        int a = 0, b = 1;
        while (true)
        {
            yield return a;
            int temp = a;
            a = b;
            b = temp + b;
        }
        // ReSharper disable once IteratorNeverReturns
    }
    
    public static IEnumerable<int> GetPrimeNumbers(int n)
    {
        if (n <= 0)
            yield break;
        yield return 1;
        if (n <= 2)
        {
            if (n == 2)
                yield return 2;
            yield break;
        }
        yield return 2;
        bool[] eratosthenes = new bool[n];
        for (int i = 3; i < n; i++)
        {
            if (i % 2 == 0)
                eratosthenes[i] = false;
            else
                eratosthenes[i] = true;
        }
        for (int i = 3; i < n; i += 2)
        {
            if (eratosthenes[i])
            {
                yield return i;
                for (int j = i * 2; j < n; j += i)
                    eratosthenes[j] = false;
            }
        }
    }
}