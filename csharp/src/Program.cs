using System.Diagnostics;
using System.Threading.Channels;
using Shallenge.CSharp;

const int MAX_THREADS = 5;
const long NUM_HASHES = 100_000_000;

var cts = new CancellationTokenSource();

var channel = Channel.CreateUnbounded<string>();

var generator = new StringGenerator(1, channel);
var processors = Enumerable.Range(0, MAX_THREADS).Select(i => new Processor(i, channel)).ToList();

var stopwatch = Stopwatch.StartNew();

var generatorTask = Task.Run(() => generator.GenerateAsync(NUM_HASHES));
var processorTasks = processors.Select(p => Task.Run(() => p.Process(cts.Token))).ToList();

var timer = new Timer((obj) => {
    Console.WriteLine($"Size of Channel: {channel.Reader.Count}");
}, null, 0, 5000);

await generatorTask;
var results = await Task.WhenAll(processorTasks);
var (Hash, Nonce) = results.MinBy(x => x.Hash, StringComparer.Ordinal);

timer.Dispose();

var hashRate = (NUM_HASHES / stopwatch.ElapsedMilliseconds) * 1000;
// This is overall time per hash without account for multithreading.
// So we multiply by the degree of paralellism to estimate the real single-threaded number
var timePerHash = stopwatch.Elapsed.TotalNanoseconds / NUM_HASHES;

var report = $"""

==============================
Overall:
Processed {NUM_HASHES} hashes in {stopwatch.ElapsedMilliseconds}ms
Performence: Total {hashRate} hashes per second, est. {timePerHash:F2}ns per hash
Shortest Hash: {Hash}
Nonce Used: {Nonce}
==============================
""";

Console.WriteLine(report);
