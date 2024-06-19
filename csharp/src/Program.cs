using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Shallenge.CSharp;

const int BATCHES = 32;
const long BATCH_SIZE = 50_000_000;
const string USERNAME = "BrownKnight/Having/Fun/With/CSharp";

var lowestHashBytes = Convert.FromHexString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
var lowestHash = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ_Z";
var nonceInLowestHash = "";
var lowestIteration = 0L;

var stopwatch = Stopwatch.StartNew();

Parallel.For(0, BATCHES, i =>
{
    var (hash, nonce, iteration) = ProcessBatch(i);
    if (string.CompareOrdinal(hash, lowestHash) < 0)
    {
        lowestHash = hash;
        nonceInLowestHash = nonce;
        lowestIteration = iteration;
    }
});

var hashRate = (BATCHES * BATCH_SIZE / stopwatch.ElapsedMilliseconds) * 1000;

var report = $"""

==============================
Overall:
Processed {BATCHES * BATCH_SIZE} hashes in {stopwatch.ElapsedMilliseconds}ms ({hashRate} hashes per second)
Shortest Hash: {lowestHash}
Nonce Used: {nonceInLowestHash} ({lowestIteration})
==============================
""";

Console.WriteLine(report);

static (string Hash, string Nonce, long Iteration) ProcessBatch(int batchNumber)
{
    var lowestHash = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ_Z";
    var nonceInLowestHash = "";
    var lowestIteration = 0L;

    var start = batchNumber * BATCH_SIZE;
    Console.WriteLine($"Start Attempt: {batchNumber} (starting at {start} with {BATCH_SIZE} iterations)");

    var generator = StringGenerator.Generate(start.ToString(), BATCH_SIZE);

    var stopwatch = Stopwatch.StartNew();

    foreach (var nonce in generator)
    {
        var stringToHash = $@"{USERNAME}/{nonce}";
        var toHash = Encoding.ASCII.GetBytes(stringToHash);

        var hashed = SHA256.HashData(toHash);
        var hashedString = Convert.ToHexString(hashed);

        if (string.CompareOrdinal(hashedString, lowestHash) < 0)
        {
            lowestHash = hashedString;
            nonceInLowestHash = stringToHash;
        }
    }

    var hashRate = (BATCH_SIZE / stopwatch.ElapsedMilliseconds) * 1000;

    var report = $"""
    Attempt {batchNumber} ({start}) Report:
    Processed {BATCH_SIZE} hashes in {stopwatch.ElapsedMilliseconds}ms ({hashRate} hashes per second)
    Shortest Hash: {lowestHash}
    Nonce Used: {nonceInLowestHash} ({lowestIteration})

    """;

    Console.WriteLine(report);

    return (lowestHash, nonceInLowestHash, lowestIteration);
}

