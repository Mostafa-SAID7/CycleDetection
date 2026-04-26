namespace CycleDetection.Abstractions;

/// <summary>
/// Defines the contract for cycle detection in 2D grids.
/// </summary>
public interface ICycleDetector
{
    /// <summary>
    /// Detects if a cycle exists in a 2D grid.
    /// </summary>
    /// <param name="grid">Flattened 1D array representing the grid</param>
    /// <param name="rows">Number of rows in the grid</param>
    /// <param name="cols">Number of columns in the grid</param>
    /// <returns>True if a cycle exists, false otherwise</returns>
    bool HasCycle(char[] grid, int rows, int cols);
}
