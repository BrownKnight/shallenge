using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Shallenge.CSharp;

const long ITERATIONS = 5;
const long BATCH_SIZE = 50_000_000;
const string USERNAME = "BrownKnight";

var lowestHashBytes = Convert.FromHexString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
Console.WriteLine(lowestHashBytes.Count());
var lowestHash = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ_Z";
var nonceInLowestHash = "";

for (var iteration = 0; iteration < ITERATIONS; iteration++)
{
    var start = iteration * BATCH_SIZE;
    var end = (iteration + 1) * BATCH_SIZE;

    Console.WriteLine($"Start Attempt: {iteration} ({start} - {end})");

    var stopwatch = Stopwatch.StartNew();

    for (var i = start; i < end; i++)
    {
        var nonce = Convert.ToBase64String(BitConverter.GetBytes(i));
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
    Attempt {iteration} Report:
    Processed {BATCH_SIZE} hashes in {stopwatch.ElapsedMilliseconds}ms ({hashRate} hashes per second)
    Shortest Hash: {lowestHash}
    Nonce Used: {nonceInLowestHash}

    """;

    Console.WriteLine(report);
}

