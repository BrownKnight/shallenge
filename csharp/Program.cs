using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

const long ITERATIONS = 10;
const long BATCH_SIZE = 10_000_000;
const string USERNAME = "BrownKnight";

var lowestHash = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ_Z";
var nonceInLowestHash = "";

for (var iteration = 0; iteration < ITERATIONS; iteration++)
{
    var start = iteration * BATCH_SIZE;
    var end = (iteration + 1) * BATCH_SIZE;

    Console.WriteLine(string.Empty);
    Console.WriteLine($"Attempt: {iteration} ({start} - {end})");

    var stopwatch = Stopwatch.StartNew();

    for (var i = start; i < end; i++)
    {
        var nonce = Convert.ToBase64String(BitConverter.GetBytes(i));
        var toHash = Encoding.ASCII.GetBytes($@"{USERNAME}/{nonce}");

        var hash = SHA256.HashData(toHash);
        var hashString = Convert.ToHexString(hash);

        if (string.CompareOrdinal(hashString, lowestHash) < 0)
        {
            lowestHash = hashString;
            nonceInLowestHash = nonce;
        }
    }

    var rate = (BATCH_SIZE / stopwatch.ElapsedMilliseconds) * 1000;
    Console.WriteLine($"Processed {BATCH_SIZE} hashes in {stopwatch.ElapsedMilliseconds}ms ({rate} hashes per second)");
    Console.WriteLine($"Shortest Hash: {lowestHash}");
    Console.WriteLine($"Nonce Used: {nonceInLowestHash}");
}

