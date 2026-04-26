using System;
using CycleDetection.Abstractions;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CycleDetection.Optimizations;

/// <summary>
/// Optimized DFS implementation using iterative approach with explicit stack.
/// </summary>
public sealed class OptimizedDfsDetector : ICycleDetector
{
    public bool HasCycle(char[] grid, int rows, int cols)
    {
        if (grid == null || grid.Length == 0 || rows <= 0 || cols <= 0)
            return false;

        var visited = new bool[grid.Length];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                int index = i * cols + j;
                if (!visited[index])
                {
                    if (HasCycleIterative(grid, visited, i, j, rows, cols, grid[index]))
                        return true;
                }
            }
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool HasCycleIterative(char[] grid, bool[] visited, int startRow, int startCol, int rows, int cols, char targetChar)
    {
        var stack = new Stack<(int row, int col, int parent)>(rows * cols);
        stack.Push((startRow, startCol, -1));

        while (stack.Count > 0)
        {
            var (row, col, parent) = stack.Pop();
            int idx = row * cols + col;

            if (visited[idx])
                continue;

            visited[idx] = true;

            // Check all 4 directions
            for (int d = 0; d < 4; d++)
            {
                int newRow = row + ((d & 1) == 0 ? (d - 1) : 0);
                int newCol = col + ((d & 1) == 1 ? (d - 2) : 0);

                if ((uint)newRow >= (uint)rows || (uint)newCol >= (uint)cols)
                    continue;

                int newIdx = newRow * cols + newCol;
                if (grid[newIdx] != targetChar || newIdx == parent)
                    continue;

                if (visited[newIdx])
                    return true; // Found cycle

                stack.Push((newRow, newCol, idx));
            }
        }

        return false;
    }
}
