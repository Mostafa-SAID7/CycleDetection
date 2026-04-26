using System;
using CycleDetection.Abstractions;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CycleDetection.Optimizations;

/// <summary>
/// Ultra-optimized DFS using iterative approach with aggressive inlining.
/// </summary>
public sealed class UnsafeDfsDetector : ICycleDetector
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
                    if (HasCycleIterativeUnsafe(grid, visited, i, j, rows, cols, grid[index]))
                        return true;
                }
            }
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe bool HasCycleIterativeUnsafe(char[] grid, bool[] visited, int startRow, int startCol, int rows, int cols, char targetChar)
    {
        var stack = new Stack<(int row, int col, int parent)>(rows * cols);
        stack.Push((startRow, startCol, -1));

        fixed (char* gridPtr = grid)
        fixed (bool* visitedPtr = visited)
        {
            while (stack.Count > 0)
            {
                var (row, col, parent) = stack.Pop();
                int idx = row * cols + col;

                if (visitedPtr[idx])
                    continue;

                visitedPtr[idx] = true;

                // Unrolled direction checks for maximum performance
                // Up
                if (row > 0)
                {
                    int newIdx = (row - 1) * cols + col;
                    if (gridPtr[newIdx] == targetChar && newIdx != parent)
                    {
                        if (visitedPtr[newIdx]) return true;
                        stack.Push((row - 1, col, idx));
                    }
                }

                // Down
                if (row < rows - 1)
                {
                    int newIdx = (row + 1) * cols + col;
                    if (gridPtr[newIdx] == targetChar && newIdx != parent)
                    {
                        if (visitedPtr[newIdx]) return true;
                        stack.Push((row + 1, col, idx));
                    }
                }

                // Left
                if (col > 0)
                {
                    int newIdx = row * cols + col - 1;
                    if (gridPtr[newIdx] == targetChar && newIdx != parent)
                    {
                        if (visitedPtr[newIdx]) return true;
                        stack.Push((row, col - 1, idx));
                    }
                }

                // Right
                if (col < cols - 1)
                {
                    int newIdx = row * cols + col + 1;
                    if (gridPtr[newIdx] == targetChar && newIdx != parent)
                    {
                        if (visitedPtr[newIdx]) return true;
                        stack.Push((row, col + 1, idx));
                    }
                }
            }
        }

        return false;
    }
}
