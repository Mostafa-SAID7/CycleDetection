using System;
using System.Collections.Generic;
using CycleDetection.Abstractions;
using CycleDetection.Core;
using CycleDetection.Optimizations;
using Xunit;

namespace CycleDetection.Tests;

public class CycleDetectorTests
{
    private static readonly ICycleDetector[] Detectors = new ICycleDetector[]
    {
        new StandardDfsDetector(),
        new OptimizedDfsDetector(),
        new UnsafeDfsDetector()
    };

    [Theory]
    [MemberData(nameof(GetDetectors))]
    public void HasCycle_WithSimpleCycle_ReturnsTrue(ICycleDetector detector)
    {
        // Arrange
        char[][] grid = new[]
        {
            new[] { 'a', 'a', 'a' },
            new[] { 'a', 'a', 'a' },
            new[] { 'a', 'a', 'a' }
        };
        var flattened = Flatten(grid);

        // Act
        bool result = detector.HasCycle(flattened, 3, 3);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(GetDetectors))]
    public void HasCycle_WithNoCycle_ReturnsFalse(ICycleDetector detector)
    {
        // Arrange
        char[][] grid = new[]
        {
            new[] { 'a', 'b', 'c' },
            new[] { 'd', 'e', 'f' },
            new[] { 'g', 'h', 'i' }
        };
        var flattened = Flatten(grid);

        // Act
        bool result = detector.HasCycle(flattened, 3, 3);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(GetDetectors))]
    public void HasCycle_WithSingleCell_ReturnsFalse(ICycleDetector detector)
    {
        // Arrange
        char[] grid = new[] { 'a' };

        // Act
        bool result = detector.HasCycle(grid, 1, 1);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(GetDetectors))]
    public void HasCycle_WithTwoCells_ReturnsFalse(ICycleDetector detector)
    {
        // Arrange
        char[] grid = new[] { 'a', 'a' };

        // Act
        bool result = detector.HasCycle(grid, 1, 2);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(GetDetectors))]
    public void HasCycle_WithLCycle_ReturnsTrue(ICycleDetector detector)
    {
        // Arrange
        char[][] grid = new[]
        {
            new[] { 'a', 'a', 'a' },
            new[] { 'a', 'b', 'a' },
            new[] { 'a', 'a', 'a' }
        };
        var flattened = Flatten(grid);

        // Act
        bool result = detector.HasCycle(flattened, 3, 3);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(GetDetectors))]
    public void HasCycle_WithEmptyGrid_ReturnsFalse(ICycleDetector detector)
    {
        // Arrange
        char[] grid = Array.Empty<char>();

        // Act
        bool result = detector.HasCycle(grid, 0, 0);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(GetDetectors))]
    public void HasCycle_WithNullGrid_ReturnsFalse(ICycleDetector detector)
    {
        // Act & Assert
        Assert.False(detector.HasCycle(null!, 0, 0));
    }

    [Theory]
    [MemberData(nameof(GetDetectors))]
    public void HasCycle_WithLargeGrid_Completes(ICycleDetector detector)
    {
        // Arrange
        int size = 100;
        var grid = new char[size * size];
        for (int i = 0; i < grid.Length; i++)
            grid[i] = 'a';

        // Act
        bool result = detector.HasCycle(grid, size, size);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(GetDetectors))]
    public void HasCycle_WithMultipleCycles_ReturnsTrue(ICycleDetector detector)
    {
        // Arrange
        char[][] grid = new[]
        {
            new[] { 'a', 'a', 'b', 'b' },
            new[] { 'a', 'a', 'b', 'b' },
            new[] { 'c', 'c', 'd', 'd' },
            new[] { 'c', 'c', 'd', 'd' }
        };
        var flattened = Flatten(grid);

        // Act
        bool result = detector.HasCycle(flattened, 4, 4);

        // Assert
        Assert.True(result);
    }

    public static IEnumerable<object[]> GetDetectors()
    {
        foreach (var detector in Detectors)
            yield return new object[] { detector };
    }

    private static char[] Flatten(char[][] grid)
    {
        int rows = grid.Length;
        int cols = grid[0].Length;
        var flattened = new char[rows * cols];

        for (int i = 0; i < rows; i++)
        {
            Array.Copy(grid[i], 0, flattened, i * cols, cols);
        }

        return flattened;
    }
}
