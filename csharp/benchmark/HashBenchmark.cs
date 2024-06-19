using System;
using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;

namespace benchmark
{
    public class HashBenchmark
    {
        private readonly SHA256 _sha256 = SHA256.Create();

        private readonly string _toHash = "BrownKnight/Having/Fun/With/CSharp000000070001xfbl";

        [Benchmark]
        public void HashWithStaticMethod()
        {
            var bytes = Encoding.ASCII.GetBytes(_toHash);
            var hashed = SHA256.HashData(bytes);
            var hashedString = Convert.ToHexString(hashed);
        }

        [Benchmark]
        public void HashWithStoredSha256()
        {
            var bytes = Encoding.ASCII.GetBytes(_toHash);
            var hashed = _sha256.ComputeHash(bytes);
            var hashedString = Convert.ToHexString(hashed);
        }
    }
}
