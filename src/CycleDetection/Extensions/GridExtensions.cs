using System;
using System.Runtime.CompilerServices;

namespace CycleDetection.Extensions;

/// <summary>
/// Extension methods for grid operations.
/// </summary>
public static class GridExtensions
{
    /// <summary>
    /// Flattens a 2D jagged array into a 1D array.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char[] Flatten(this char[][] grid)
    {
        if (grid == null || grid.Length == 0)
            return Array.Empty<char>();

        int rows = grid.Length;
        int cols = grid[0].Length;
        var flattened = new char[rows * cols];

        for (int i = 0; i < rows; i++)
        {
            Array.Copy(grid[i], 0, flattened, i * cols, cols);
        }

        return flattened;
    }

    /// <summary>
    /// Flattens a 2D jagged array into a span.
    /// </summary>
    public static void FlattenToSpan(this char[][] grid, Span<char> destination)
    {
        if (grid == null || grid.Length == 0)
            return;

        int rows = grid.Length;
        int cols = grid[0].Length;

        for (int i = 0; i < rows; i++)
        {
            grid[i].AsSpan().CopyTo(destination.Slice(i * cols, cols));
        }
    }

    /// <summary>
    /// Gets the index in a flattened array from row and column.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetFlatIndex(int row, int col, int cols) => row * cols + col;

    /// <summary>
    /// Gets row and column from a flattened index.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (int row, int col) GetRowCol(int index, int cols) => (index / cols, index % cols);
}
