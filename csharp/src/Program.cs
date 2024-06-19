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

var stopwatch = Stopwatch.StartNew();

Parallel.For(0, BATCHES, i =>
{
    var (hash, nonce) = ProcessBatch(i);
    if (string.CompareOrdinal(hash, lowestHash) < 0)
    {
        lowestHash = hash;
        nonceInLowestHash = nonce;
    }
});

var hashRate = (BATCHES * BATCH_SIZE / stopwatch.ElapsedMilliseconds) * 1000;
var timePerHash = stopwatch.Elapsed.TotalNanoseconds / BATCH_SIZE;

var report = $"""

==============================
Overall:
Processed {BATCHES * BATCH_SIZE} hashes in {stopwatch.ElapsedMilliseconds}ms
Performence: {hashRate} hashes per second, {timePerHash}ns per hash
Shortest Hash: {lowestHash}
Nonce Used: {nonceInLowestHash}
==============================
""";

Console.WriteLine(report);

static (string Hash, string Nonce) ProcessBatch(int batchNumber)
{
    var sha256 = SHA256.Create();
    var lowestHash = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ_Z";
    var nonceInLowestHash = "";

    var start = batchNumber * BATCH_SIZE;
    Console.WriteLine($"Start Attempt: {batchNumber} (starting at {start} with {BATCH_SIZE} iterations)");

    var generator = StringGenerator.Generate(USERNAME, start.ToString(), BATCH_SIZE);

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
    Processed {BATCH_SIZE} hashes in {stopwatch.ElapsedMilliseconds}ms
    Shortest Hash: {lowestHash}
    Nonce Used: {nonceInLowestHash}

    """;

    Console.WriteLine(report);

    return (lowestHash, nonceInLowestHash);
}

