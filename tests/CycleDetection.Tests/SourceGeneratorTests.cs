using CycleDetection.Extensions;
using Xunit;

namespace CycleDetection.Tests;

public class SourceGeneratorTests
{
    [Fact]
    public void FlattenFast_ShouldCorrectlyFlattenJaggedArray()
    {
        // Arrange
        char[][] grid2D = new[]
        {
            new[] { 'a', 'b', 'c' },
            new[] { 'd', 'e', 'f' },
            new[] { 'g', 'h', 'i' }
        };
        char[] expected = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i' };

        // Act
        char[] result = grid2D.FlattenFast();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void FlattenFast_ShouldHandleEmptyGrid()
    {
        // Arrange
        char[][] grid2D = System.Array.Empty<char[]>();

        // Act
        char[] result = grid2D.FlattenFast();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void FlattenFast_ShouldHandleNullGrid()
    {
        // Arrange
        char[][]? grid2D = null;

        // Act
        char[] result = grid2D!.FlattenFast();

        // Assert
        Assert.Empty(result);
    }
}
