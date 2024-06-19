using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Shallenge.CSharp;

public static class Processor
{
    private static readonly SHA256 s_sha256 = SHA256.Create();
    private const string USERNAME = "BrownKnight/Having/Fun/With/CSharp";

    public static (string Hash, string Nonce) ProcessBatch(int batchNumber, long batchSize)
    {
        var lowestHash = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ_Z";
        var nonceInLowestHash = "";

        var start = batchNumber * batchSize;
        Console.WriteLine($"Start Attempt: {batchNumber} (starting at {start} with {batchSize} iterations)");

        var generator = StringGenerator.Generate(USERNAME, start.ToString(), batchSize);

        var stopwatch = Stopwatch.StartNew();

        foreach (var stringToHash in generator)
        {
            HashAndCheck(ref lowestHash, ref nonceInLowestHash, stringToHash);
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

    public static void HashAndCheck(ref string lowestHash, ref string nonceInLowestHash, string stringToHash)
    {
        var toHash = Encoding.ASCII.GetBytes(stringToHash);
        var hashed = s_sha256.ComputeHash(toHash);
        var hashedString = Convert.ToHexString(hashed);

        if (string.CompareOrdinal(hashedString, lowestHash) < 0)
        {
            lowestHash = hashedString;
            nonceInLowestHash = stringToHash;
        }
    }
}