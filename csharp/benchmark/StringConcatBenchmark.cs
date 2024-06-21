using System;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;

namespace benchmark
{
    public class StringCompareBenchmark
    {
        private const string StrA = "00000004699512927ED7B210F2B869A67D54C1D3A28BD68EC13C4A48A9F5BFAC";
        private const string StrB = "00000000699512927ED7B210F2B869A67D54C1D3A28BD68EC13C4A48A9F5BFAC";

        [Benchmark]
        public void NormalCompareOrdinal()
        {
            var result = string.CompareOrdinal(StrA, StrB) < 0;
        }

        [Benchmark]
        public void LimitedCompareOrdinal()
        {
            var result = string.CompareOrdinal(StrA, 0, StrB, 0, 10) < 0;
        }

        [Benchmark]
        public void Compare()
        {
            var result = string.Compare(StrA, StrB) < 0;
        }
    }
}
