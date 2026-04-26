# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-04-26

### Added
- Initial release of CycleDetection library.
- `ICycleDetector` interface for standardizing cycle detection in 2D grids.
- Three implementation strategies:
  - `StandardDfsDetector`: Baseline recursive DFS.
  - `OptimizedDfsDetector`: Iterative DFS using `Span<T>` and `stackalloc`.
  - `UnsafeDfsDetector`: High-performance detector using pointer arithmetic and aggressive inlining.
- Dependency injection support with `AddCycleDetection` extension method.
- `GridExtensions` for easy flattening of 2D grids.
- Roslyn Source Generator for high-performance `FlattenFast` method.
- Comprehensive unit tests with xUnit.
- Benchmark suite using BenchmarkDotNet.
- GitHub Actions CI/CD workflow for automated building, testing, and NuGet publishing.
