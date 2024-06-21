using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Shallenge.CSharp;

public sealed class Processor
{
    private readonly SHA256 _sha256 = SHA256.Create();
    private readonly StringGenerator _generator = new StringGenerator();
    private const string USERNAME = "BrownKnight/Having/Fun/With/CSharp+Apple/Silicon/Mac";

    public (string Hash, string Nonce) ProcessBatch(int batchNumber, long batchSize)
    {
        var lowestHash = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ_Z";
        var nonceInLowestHash = "";

        var start = batchNumber * batchSize;
        Console.WriteLine($"Start Batch: {batchNumber} (starting at {start} with {batchSize} iterations)");

        var generator = _generator.Generate(USERNAME, start.ToString(), batchSize);

        var stopwatch = Stopwatch.StartNew();

        foreach (var stringToHash in generator)
        {
            HashAndCheck(ref lowestHash, ref nonceInLowestHash, stringToHash);
        }

        var hashRate = (batchSize / stopwatch.ElapsedMilliseconds) * 1000;
        var timePerHash = stopwatch.Elapsed.TotalNanoseconds / batchSize;

        var report = $"""
        Batch {batchNumber} ({start}) Report:
        Processed {batchSize} hashes in {stopwatch.ElapsedMilliseconds}ms
        Performence: {hashRate} hashes per second, {timePerHash:F2}ns per hash
        Shortest Hash: {lowestHash}
        Nonce Used: {nonceInLowestHash}

        """;

        Console.WriteLine(report);

        return (lowestHash, nonceInLowestHash);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void HashAndCheck(ref string lowestHash, ref string nonceInLowestHash, string stringToHash)
    {
        var toHash = Encoding.ASCII.GetBytes(stringToHash);
        var hashed = _sha256.ComputeHash(toHash);
        var hashedString = Convert.ToHexString(hashed);

        if (string.CompareOrdinal(hashedString, 0, lowestHash, 0, 10) < 0)
        {
            lowestHash = hashedString;
            nonceInLowestHash = stringToHash;
        }
    }
}