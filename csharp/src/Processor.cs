using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Shallenge.CSharp;

public static class Processor
{
    private const string USERNAME = "BrownKnight/Having/Fun/With/CSharp";

    public static (string Hash, string Nonce) ProcessBatch(int batchNumber, long batchSize)
    {
        var sha256 = SHA256.Create();
        var lowestHash = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ_Z";
        var nonceInLowestHash = "";

        var start = batchNumber * batchSize;
        Console.WriteLine($"Start Attempt: {batchNumber} (starting at {start} with {batchSize} iterations)");

        var generator = StringGenerator.Generate(USERNAME, start.ToString(), batchSize);

        var stopwatch = Stopwatch.StartNew();

        foreach (var stringToHash in generator)
        {
            var toHash = Encoding.ASCII.GetBytes(stringToHash);
            var hashed = sha256.ComputeHash(toHash);
            var hashedString = Convert.ToHexString(hashed);

            if (string.CompareOrdinal(hashedString, lowestHash) < 0)
            {
                lowestHash = hashedString;
                nonceInLowestHash = stringToHash;
            }
        }

        var report = $"""
    Attempt {batchNumber} ({start}) Report:
    Processed {batchSize} hashes in {stopwatch.ElapsedMilliseconds}ms
    Shortest Hash: {lowestHash}
    Nonce Used: {nonceInLowestHash}

    """;

        Console.WriteLine(report);

        return (lowestHash, nonceInLowestHash);
    }
}