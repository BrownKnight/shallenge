using System.Linq;
using BenchmarkDotNet.Attributes;
using Shallenge.CSharp;

namespace benchmark
{
    public class StringGeneratorBenchmark
    {
        private readonly StringGenerator generator = new StringGenerator();

        [Benchmark]
        public void OneString()
        {
            generator.Generate("SOME_PREFIX", "0", 1).ToArray();
        }

        [Benchmark]
        public void OneHundredStrings()
        {
            generator.Generate("SOME_PREFIX", "0", 100).ToArray();
        }
    }
}
