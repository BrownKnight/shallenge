namespace Shallenge.CSharp;

public static class RangeGenerator 
{
    public static IEnumerable<long> Range(long start, long count)
    {
        var end = start + count;
        for (var i = start; i < end; i++)
        {
            yield return i;
        }
    }
}