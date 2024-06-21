using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;

namespace Shallenge.CSharp;

public sealed class Processor(int id, Channel<string> channel)
{
    public (string Hash, string Nonce) Process()
    {
        Console.WriteLine($"Start Processor {id}");

        var lowestHash = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ_Z";
        var nonceInLowestHash = "";
        using var sha256 = SHA256.Create();

        while (channel.Reader.TryRead(out var stringToHash))
        {
            var toHash = Encoding.ASCII.GetBytes(stringToHash);
            var hashed = sha256.ComputeHash(toHash);
            var hashedString = Convert.ToHexString(hashed);

            if (string.CompareOrdinal(hashedString, 0, lowestHash, 0, 10) < 0)
            {
                lowestHash = hashedString;
                nonceInLowestHash = stringToHash;
            }
        }

        var report = $"""
        Processor {id} Report:
        Shortest Hash: {lowestHash}
        Nonce Used: {nonceInLowestHash}

        """;

        Console.WriteLine(report);

        return (lowestHash, nonceInLowestHash);
    }
}