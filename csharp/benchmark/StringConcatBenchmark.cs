using System;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;

namespace benchmark
{
    public class StringConcatBenchmark
    {
        private const string Prefix = "SomeStringPrefix";

        [Params("12345678")]
        public string Suffix;

        [Benchmark]
        public void InterpolationConcat()
        {
            var result = $"{Prefix}/{Suffix}";
        }

        [Benchmark]
        public void DirectConcat()
        {
            var result = Prefix + Suffix;
        }
    }
}
