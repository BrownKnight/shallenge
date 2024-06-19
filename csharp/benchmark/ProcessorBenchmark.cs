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

        private StringGenerator stringGenerator = new StringGenerator();
        private Processor processor = new Processor();

        [Benchmark]
        public void WithStringGenerator()
        {
            var toHash = stringGenerator.Generate("BrownKnight/Having/Fun/With/CSharp", "0", 1).First();
            processor.HashAndCheck(ref lowestHash, ref lowestNonse, toHash);
        }

        [Benchmark]
        public void WithConstantString()
        {
            processor.HashAndCheck(ref lowestHash, ref lowestNonse, toHash);
        }
    }
}
