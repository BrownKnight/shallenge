using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;

namespace Shallenge.CSharp;

public sealed class Processor(int id, Channel<string> channel)
{
    public async Task<(string Hash, string Nonce)> Process(CancellationToken cancellationToken)
    {
        Console.WriteLine($"Start Processor {id}");

        var lowestHash = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ_Z";
        var nonceInLowestHash = "";
        using var sha256 = SHA256.Create();

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var stringToHash = await channel.Reader.ReadAsync(cancellationToken);
                var toHash = Encoding.ASCII.GetBytes(stringToHash);
                var hashed = sha256.ComputeHash(toHash);
                var hashedString = Convert.ToHexString(hashed);

                if (string.CompareOrdinal(hashedString, 0, lowestHash, 0, 10) < 0)
                {
                    lowestHash = hashedString;
                    nonceInLowestHash = stringToHash;
                }
            } catch (Exception e)
            {
                // do nothing
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