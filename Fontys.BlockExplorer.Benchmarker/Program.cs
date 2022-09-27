using BenchmarkDotNet.Running;
using MyBenchmarks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
