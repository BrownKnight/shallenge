namespace Shallenge.CSharp;

public static class StringGenerator {

    private const int LENGTH = 16;

    public static IEnumerable<string> Generate(string prefix, string initial, long iterations)
    {
        var chars = $"{prefix}{initial.PadLeft(LENGTH, '0')}".ToCharArray();

        for (var iteration = 0; iteration < iterations; iteration++)
        {
            var index = chars.Length - 1;
            while (index > 0)
            {
                chars[index] = NextChar(chars[index]);

                // If char is 0, we've rolled over and need to carry the number
                if (chars[index] != '0') break;
                index--;
            }

            yield return new string(chars);
        }
    }

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