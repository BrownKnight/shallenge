using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Shallenge.CSharp;

public sealed class StringGenerator(int id, int count, Queue<string> queue)
{
    private const int LENGTH = 12;
    private const string USERNAME = "BrownKnight/Having/Fun/With/CSharp+Apple/Silicon/Mac";

    public void Generate()
    {
        var stopwatch = Stopwatch.StartNew();
        Console.WriteLine($"Starting Generator {id} for {count} strings");
        var chars = $"{USERNAME}{id.ToString().PadRight(LENGTH, '0')}".ToCharArray();
        var strings = new string[count];

        for(var i = 0L; i < count; i++)
        {
            var index = chars.Length - 1;
            while (index > 0)
            {
                chars[index] = NextChar(chars[index]);

                // If char is 0, we've rolled over and need to carry the number
                if (chars[index] != '0') break;
                index--;
            }

            queue.Enqueue(new string(chars));
        }

        var genRate = (count / stopwatch.ElapsedMilliseconds) * 1000;
        var timePerGen = stopwatch.Elapsed.TotalNanoseconds / count;

        var report = $"""
        ++++++++++++++++++++++++++
        Generator {id}
        Generated {count} strings in {stopwatch.ElapsedMilliseconds}ms
        Performence: Total {genRate} generations per second, est. {timePerGen:F2}ns per gen
        ++++++++++++++++++++++++++
        """;

        Console.WriteLine(report);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static char NextChar(char c) => c switch
    {
        >= '0' and < '9' => ++c,
        '9' => 'A',
        >= 'A' and < 'Z' => ++c,
        'Z' => 'a',
        >= 'a' and < 'z' => ++c,
        'z' => '+',
        '+' => '/',
        _ => '0' // Need to roll over
    };
}