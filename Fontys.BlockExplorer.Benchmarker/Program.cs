using BenchmarkDotNet.Running;

namespace Fontys.BlockExplorer.Benchmarker
{
    class Program
    {
        static void Main(string[] args)
        {
            var results = BenchmarkRunner.Run<AddressRestoreBenchmarker>();
        }
    }
}
