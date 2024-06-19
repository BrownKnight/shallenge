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
    public class ProcessorBenchmark
    {
        private const string toHash = "BrownKnight/Having/Fun/With/CSharp000000070001xfbl";
        private string lowestHash = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ_Z";
        private string lowestNonse = "";

        [Benchmark]
        public void WithStringGenerator()
        {
            var toHash = StringGenerator.Generate("BrownKnight/Having/Fun/With/CSharp", "0", 1).First();
            Processor.HashAndCheck(ref lowestHash, ref lowestNonse, toHash);
        }

        [Benchmark]
        public void WithConstantString()
        {
            Processor.HashAndCheck(ref lowestHash, ref lowestNonse, toHash);
        }
    }
}
