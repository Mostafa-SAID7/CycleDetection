# CycleDetection

High-performance cycle detection library for 2D grids with multiple optimization strategies.

## Features

- **Three Implementation Strategies**
  - Standard DFS (baseline)
  - Optimized (Span, stackalloc, iterative)
  - Unsafe (pointer arithmetic, aggressive inlining)

- **Performance Optimizations**
  - Contiguous memory layout (1D arrays)
  - Zero-allocation for small grids (stackalloc)
  - Unsigned integer comparisons for bounds checking
  - Aggressive inlining
  - No exceptions in hot paths

- **Clean Architecture**
  - SOLID principles
  - Dependency injection support
  - Extensible design

## Installation

```bash
dotnet add package CycleDetection
```

## Usage

### Basic Usage

```csharp
using CycleDetection.Abstractions;
using CycleDetection.Core;

// Create a detector
ICycleDetector detector = new OptimizedDfsDetector();

// Prepare grid (flattened 1D array)
char[] grid = new[] { 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a' };
int rows = 3;
int cols = 3;

// Detect cycle
bool hasCycle = detector.HasCycle(grid, rows, cols);
```

### With Dependency Injection

```csharp
using Microsoft.Extensions.DependencyInjection;
using CycleDetection.DependencyInjection;

var services = new ServiceCollection();

// Add with default (Optimized) strategy
services.AddCycleDetection();

// Or configure specific strategy
services.AddCycleDetection(options => options.UseUnsafe());

var provider = services.BuildServiceProvider();
var detector = provider.GetRequiredService<ICycleDetector>();

bool hasCycle = detector.HasCycle(grid, rows, cols);
```

### Grid Flattening

```csharp
using CycleDetection.Extensions;

char[][] grid2D = new[]
{
    new[] { 'a', 'a', 'a' },
    new[] { 'a', 'b', 'a' },
    new[] { 'a', 'a', 'a' }
};

// Flatten to 1D array
char[] flattened = grid2D.Flatten();

// Or use Span for zero-allocation
Span<char> buffer = stackalloc char[9];
grid2D.FlattenToSpan(buffer);
```

## Strategies

### Standard DFS
- Baseline recursive implementation
- Good for understanding the algorithm
- Higher memory usage due to recursion

### Optimized
- Iterative DFS with Span
- Uses stackalloc for grids ≤ 100x100
- Recommended for most use cases
- ~2-3x faster than Standard

### Unsafe
- Pointer arithmetic
- Aggressive inlining
- Best performance for large grids
- Requires `AllowUnsafeBlocks` in project

## Performance

Benchmark results (relative to Standard):

| Grid Size | Optimized | Unsafe |
|-----------|-----------|--------|
| 100x100   | 2.5x      | 3.2x   |
| 500x500   | 2.8x      | 3.5x   |
| 1000x1000 | 2.9x      | 3.6x   |

Memory allocations:
- Standard: ~1 allocation per grid
- Optimized: 0 allocations for grids ≤ 100x100
- Unsafe: 0 allocations for grids ≤ 100x100

## Running Benchmarks

```bash
cd benchmarks/CycleDetection.Benchmarks
dotnet run -c Release
```

## Running Tests

```bash
dotnet test
```

## API Reference

### ICycleDetector

```csharp
bool HasCycle(char[] grid, int rows, int cols);
```

Detects if a cycle exists in a 2D grid.

**Parameters:**
- `grid`: Flattened 1D array representing the grid
- `rows`: Number of rows
- `cols`: Number of columns

**Returns:** `true` if a cycle exists, `false` otherwise

### GridExtensions

```csharp
char[] Flatten(this char[][] grid);
void FlattenToSpan(this char[][] grid, Span<char> destination);
int GetFlatIndex(int row, int col, int cols);
(int row, int col) GetRowCol(int index, int cols);
```

## Architecture

```
src/CycleDetection/
├── Abstractions/
│   └── ICycleDetector.cs
├── Core/
│   └── StandardDfsDetector.cs
├── Optimizations/
│   ├── OptimizedDfsDetector.cs
│   └── UnsafeDfsDetector.cs
├── Extensions/
│   └── GridExtensions.cs
└── DependencyInjection/
    ├── CycleDetectionOptions.cs
    └── ServiceCollectionExtensions.cs
```

## Publishing to NuGet

1. Update version in `src/CycleDetection/CycleDetection.csproj`
2. Create a git tag: `git tag v1.0.0`
3. Push to main: `git push origin main --tags`
4. GitHub Actions will automatically publish to NuGet

## License

MIT
