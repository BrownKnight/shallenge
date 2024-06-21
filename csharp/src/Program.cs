using System.Diagnostics;
using Shallenge.CSharp;

const int BATCHES = 128;
const int BATCH_OFFSET = 128;
const long BATCH_SIZE = 1_073_741_824; // 64^5 so that we are always changing the last 5 chars
// const long BATCH_SIZE = 16_777_216; // 64^4 so that we are always changing the last 4 chars
const long TOTAL_HASHES = BATCHES * BATCH_SIZE;

const int MAX_THREADS = 4;

var lockObj = new object();

var lowestHashBytes = Convert.FromHexString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
var lowestHash = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ_Z";
var nonceInLowestHash = "";

var stopwatch = Stopwatch.StartNew();

var options = new ParallelOptions { MaxDegreeOfParallelism = MAX_THREADS };
Parallel.For(0, BATCHES, options, i =>
{
    var processor = new Processor();
    var (hash, nonce) = processor.ProcessBatch(BATCH_OFFSET + i, BATCH_SIZE);
    lock (lockObj)
    {
        if (string.CompareOrdinal(hash, lowestHash) < 0)
        {
            lowestHash = hash;
            nonceInLowestHash = nonce;
        }
    }
});

var hashRate = (TOTAL_HASHES / stopwatch.ElapsedMilliseconds) * 1000;
// This is overall time per hash without account for multithreading.
// So we multiply by the degree of paralellism to estimate the real single-threaded number
var timePerHash = stopwatch.Elapsed.TotalNanoseconds / TOTAL_HASHES * MAX_THREADS;

var report = $"""

==============================
Overall:
Processed {TOTAL_HASHES} hashes in {stopwatch.ElapsedMilliseconds}ms
Performence: Total {hashRate} hashes per second, est. {timePerHash:F2}ns per hash
Shortest Hash: {lowestHash}
Nonce Used: {nonceInLowestHash}
==============================
""";

Console.WriteLine(report);
