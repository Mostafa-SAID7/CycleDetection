using CycleDetection.Abstractions;
using System;

namespace CycleDetection.Core;

/// <summary>
/// Standard DFS-based cycle detection implementation (baseline).
/// </summary>
public sealed class StandardDfsDetector : ICycleDetector
{
    /// <summary>
    /// Detects if a cycle exists in a 2D grid using standard DFS approach.
    /// </summary>
    /// <param name="grid">Flattened 1D array representing the grid</param>
    /// <param name="rows">Number of rows in the grid</param>
    /// <param name="cols">Number of columns in the grid</param>
    /// <returns>True if a cycle exists, false otherwise</returns>
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
                    if (DfsHasCycle(grid, visited, i, j, -1, rows, cols, grid[index]))
                        return true;
                }
            }
        }

        return false;
    }

    private static bool DfsHasCycle(char[] grid, bool[] visited, int row, int col, int parent, int rows, int cols, char targetChar)
    {
        int idx = row * cols + col;
        visited[idx] = true;

        // Check all 4 directions
        int[] dRow = { -1, 1, 0, 0 };
        int[] dCol = { 0, 0, -1, 1 };

        for (int d = 0; d < 4; d++)
        {
            int newRow = row + dRow[d];
            int newCol = col + dCol[d];

            if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols)
                continue;

            int newIdx = newRow * cols + newCol;
            if (grid[newIdx] != targetChar)
                continue;

            if (newIdx == parent)
                continue;

            if (visited[newIdx])
                return true; // Found cycle

            if (DfsHasCycle(grid, visited, newRow, newCol, idx, rows, cols, targetChar))
                return true;
        }

        return false;
    }
}
