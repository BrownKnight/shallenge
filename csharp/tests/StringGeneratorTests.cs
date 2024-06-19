using Xunit;

namespace Shallenge.CSharp;

public class StringGeneratorTests
{
    [Theory]
    [InlineData('0', '1')]
    [InlineData('1', '2')]
    [InlineData('9', 'A')]
    [InlineData('A', 'B')]
    [InlineData('Z', 'a')]
    [InlineData('a', 'b')]
    [InlineData('z', '0')]
    public void VerifyNextChar(char initial, char expected)
    {
        var generator = StringGenerator.Generate(initial.ToString(), 1);
        var next = generator.Take(1).First();

        Assert.Equal(expected, next.Last());
    }
}