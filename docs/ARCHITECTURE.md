# Architecture

## Design Principles

### SOLID Principles

**Single Responsibility**
- Each detector implementation handles one strategy
- Extensions handle grid operations
- DI handles configuration

**Open/Closed**
- Open for extension (new detector implementations)
- Closed for modification (interface contract)

**Liskov Substitution**
- All implementations satisfy ICycleDetector contract
- Can swap implementations without changing client code

**Interface Segregation**
- Minimal ICycleDetector interface
- No unnecessary methods

**Dependency Inversion**
- Depend on ICycleDetector abstraction
- DI container manages concrete implementations

### Clean Architecture

```
┌─────────────────────────────────────┐
│      Presentation Layer             │
│  (DependencyInjection)              │
└─────────────────────────────────────┘
           ↓
┌─────────────────────────────────────┐
│      Application Layer              │
│  (Extensions, Options)              │
└─────────────────────────────────────┘
           ↓
┌─────────────────────────────────────┐
│      Domain Layer                   │
│  (Abstractions)                     │
└─────────────────────────────────────┘
           ↓
┌─────────────────────────────────────┐
│      Infrastructure Layer           │
│  (Core, Optimizations)              │
└─────────────────────────────────────┘
```

## Project Structure

### src/CycleDetection/

**Abstractions/**
- `ICycleDetector.cs` - Core interface defining the contract

**Core/**
- `StandardDfsDetector.cs` - Baseline recursive implementation

**Optimizations/**
- `OptimizedDfsDetector.cs` - Span-based iterative implementation
- `UnsafeDfsDetector.cs` - Pointer-based implementation

**Extensions/**
- `GridExtensions.cs` - Utility methods for grid operations

**DependencyInjection/**
- `CycleDetectionOptions.cs` - Configuration options
- `ServiceCollectionExtensions.cs` - DI registration

### tests/CycleDetection.Tests/

- `CycleDetectorTests.cs` - Comprehensive unit tests
- Theory tests for all implementations
- Edge case coverage

### benchmarks/CycleDetection.Benchmarks/

- `CycleDetectionBenchmarks.cs` - Performance benchmarks
- Compares all three implementations
- Multiple grid sizes

## Dependency Graph

```
ICycleDetector (interface)
    ↑
    ├── StandardDfsDetector
    ├── OptimizedDfsDetector
    └── UnsafeDfsDetector

ServiceCollectionExtensions
    ↓
CycleDetectionOptions
    ↓
CycleDetectionStrategy (enum)

GridExtensions
    (no dependencies)
```

## Configuration Flow

```
User Code
    ↓
services.AddCycleDetection(options => options.UseOptimized())
    ↓
ServiceCollectionExtensions.AddCycleDetection()
    ↓
CycleDetectionOptions (Strategy = Optimized)
    ↓
Factory creates OptimizedDfsDetector
    ↓
ICycleDetector registered in DI container
    ↓
User retrieves via GetRequiredService<ICycleDetector>()
```

## Memory Model

### Standard Implementation
```
Stack:
├── Recursion frames (variable depth)
└── Local variables

Heap:
└── state array (rows × cols)
```

### Optimized Implementation
```
Stack (small grids ≤ 100x100):
├── state array (stackalloc)
├── stack array (stackalloc)
└── Local variables

Heap (large grids):
├── state array
└── stack array
```

### Unsafe Implementation
```
Stack (small grids ≤ 100x100):
├── state array (stackalloc)
├── stack array (stackalloc)
└── Local variables + pointers

Heap (large grids):
├── state array
└── stack array
```

## Extension Points

### Adding New Implementation

1. Create class implementing `ICycleDetector`
2. Add strategy to `CycleDetectionStrategy` enum
3. Update factory in `ServiceCollectionExtensions`
4. Add tests in `CycleDetectorTests`
5. Add benchmarks in `CycleDetectionBenchmarks`

Example:
```csharp
public sealed class ParallelDfsDetector : ICycleDetector
{
    public bool HasCycle(char[] grid, int rows, int cols)
    {
        // Implementation using Parallel.For
    }
}
```

### Adding New Extension Method

1. Add method to `GridExtensions`
2. Mark with `[MethodImpl(MethodImplOptions.AggressiveInlining)]` if hot path
3. Add unit tests

## Performance Considerations

### Hot Paths
- `HasCycle()` method
- Neighbor processing loop
- Bounds checking

### Optimization Strategies
- Inline hot methods
- Use unsigned comparisons
- Minimize allocations
- Avoid exceptions

### Profiling Points
- Memory allocations
- CPU cache misses
- Branch mispredictions
- Method call overhead

## Testing Strategy

### Unit Tests
- All implementations tested with same test cases
- Theory tests for parameterization
- Edge cases: empty grid, single cell, no cycle, multiple cycles

### Benchmarks
- Compare all implementations
- Multiple grid sizes
- Memory allocation tracking
- Execution time measurement

### Integration Tests
- DI container configuration
- Strategy switching
- Extension methods

## Versioning

### Semantic Versioning
- MAJOR: Breaking API changes
- MINOR: New features (backward compatible)
- PATCH: Bug fixes

### NuGet Package
- Version in `.csproj`
- Git tags for releases
- GitHub Actions for publishing

## Documentation

### Code Documentation
- XML comments on public APIs
- Performance notes in comments
- Usage examples in README

### Architecture Documentation
- This file (ARCHITECTURE.md)
- PERFORMANCE.md for optimization details
- README.md for usage guide
