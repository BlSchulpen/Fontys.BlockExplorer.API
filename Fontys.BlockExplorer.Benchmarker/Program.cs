using BenchmarkDotNet.Running;
using MyBenchmarks;

namespace Fontys.BlockExplorer.Benchmarker
{
    class Program
    {
        static void Main(string[] args)
        {
            var results = BenchmarkRunner.Run<AddressRefactorBenchmarker>();
        }
    }
}
