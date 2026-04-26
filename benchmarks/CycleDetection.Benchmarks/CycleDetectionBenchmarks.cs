using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using CycleDetection.Abstractions;
using CycleDetection.Core;
using CycleDetection.Optimizations;

namespace CycleDetection.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(warmupCount: 3, iterationCount: 5)]
public class CycleDetectionBenchmarks
{
    private char[] _grid100x100 = null!;
    private char[] _grid500x500 = null!;
    private char[] _grid1000x1000 = null!;

    private ICycleDetector _standardDetector = null!;
    private ICycleDetector _optimizedDetector = null!;
    private ICycleDetector _unsafeDetector = null!;

    [GlobalSetup]
    public void Setup()
    {
        _standardDetector = new StandardDfsDetector();
        _optimizedDetector = new OptimizedDfsDetector();
        _unsafeDetector = new UnsafeDfsDetector();

        _grid100x100 = CreateGrid(100, 100);
        _grid500x500 = CreateGrid(500, 500);
        _grid1000x1000 = CreateGrid(1000, 1000);
    }

    [Benchmark]
    public bool Standard_100x100() => _standardDetector.HasCycle(_grid100x100, 100, 100);

    [Benchmark]
    public bool Optimized_100x100() => _optimizedDetector.HasCycle(_grid100x100, 100, 100);

    [Benchmark]
    public bool Unsafe_100x100() => _unsafeDetector.HasCycle(_grid100x100, 100, 100);

    [Benchmark]
    public bool Standard_500x500() => _standardDetector.HasCycle(_grid500x500, 500, 500);

    [Benchmark]
    public bool Optimized_500x500() => _optimizedDetector.HasCycle(_grid500x500, 500, 500);

    [Benchmark]
    public bool Unsafe_500x500() => _unsafeDetector.HasCycle(_grid500x500, 500, 500);

    [Benchmark]
    public bool Standard_1000x1000() => _standardDetector.HasCycle(_grid1000x1000, 1000, 1000);

    [Benchmark]
    public bool Optimized_1000x1000() => _optimizedDetector.HasCycle(_grid1000x1000, 1000, 1000);

    [Benchmark]
    public bool Unsafe_1000x1000() => _unsafeDetector.HasCycle(_grid1000x1000, 1000, 1000);

    private static char[] CreateGrid(int rows, int cols)
    {
        var grid = new char[rows * cols];
        for (int i = 0; i < grid.Length; i++)
            grid[i] = 'a';
        return grid;
    }
}

class Program
{
    static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<CycleDetectionBenchmarks>();
    }
}
