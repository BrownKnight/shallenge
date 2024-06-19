using System.Diagnostics;
using Shallenge.CSharp;

const int BATCHES = 32;
const long BATCH_SIZE = 50_000_000;

var lowestHashBytes = Convert.FromHexString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
var lowestHash = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ_Z";
var nonceInLowestHash = "";

var stopwatch = Stopwatch.StartNew();

Parallel.For(0, BATCHES, i =>
{
    var (hash, nonce) = Processor.ProcessBatch(i, BATCH_SIZE);
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
