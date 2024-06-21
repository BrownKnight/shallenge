using System.Diagnostics;
using Shallenge.CSharp;

const int MAX_QUEUES = 1;
const int NUM_HASHES = 100_000_000;
const int HASHES_PER_QUEUE = NUM_HASHES / MAX_QUEUES;
const int PROCESSORS_PER_QUEUE = 8;

var queues = Enumerable.Range(0, MAX_QUEUES).Select(i => new Queue<string>(HASHES_PER_QUEUE)).ToArray();

var generators = queues
    .Select((queue, i) => new StringGenerator(i, HASHES_PER_QUEUE, queue))
    .ToList();
var processors = queues
    .SelectMany((queue, i) => 
        Enumerable.Range(0, PROCESSORS_PER_QUEUE)
            .Select(j => new Processor($"{i}-{j}", queue)))
    .ToList();

var stopwatch = Stopwatch.StartNew();

var generatorTasks = generators.Select(g => Task.Run(() => g.Generate())).ToList();
await Task.Delay(2000);
var processorTasks = processors.Select(p => Task.Run(() => p.Process())).ToList();

var timer = new Timer((obj) => {
    queues.Select((queue, i) => { Console.WriteLine($"Size of Queue {i}: {queue.Count}"); return 0; }).ToList();
}, null, 0, 5000);

await Task.WhenAll(generatorTasks);
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
