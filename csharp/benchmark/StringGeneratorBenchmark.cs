using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using Microsoft.Diagnostics.Tracing.Stacks;
using Shallenge.CSharp;

namespace benchmark
{
    public class StringGeneratorBenchmark
    {
        [Benchmark]
        public void OneString()
        {
            StringGenerator.Generate("SOME_PREFIX", "0", 1).ToArray();

        }

        [Benchmark]
        public void OneHundredStrings()
        {
            StringGenerator.Generate("SOME_PREFIX", "0", 100).ToArray();
        }
    }
}
