using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace Shallenge.CSharp;

public sealed class StringGenerator(int id, Channel<string> channel)
{
    private const int LENGTH = 12;
    private const string USERNAME = "BrownKnight/Having/Fun/With/CSharp+Apple/Silicon/Mac";

    public async void GenerateAsync(long count)
    {
        var chars = $"{USERNAME}{id.ToString().PadRight(LENGTH, '0')}".ToCharArray();

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

            await channel.Writer.WriteAsync(new string(chars));
        }

        channel.Writer.Complete();
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