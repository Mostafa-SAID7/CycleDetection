namespace CycleDetection.DependencyInjection;

/// <summary>
/// Configuration options for cycle detection.
/// </summary>
public class CycleDetectionOptions
{
    /// <summary>
    /// Gets or sets the implementation strategy.
    /// </summary>
    public CycleDetectionStrategy Strategy { get; set; } = CycleDetectionStrategy.Optimized;
}

/// <summary>
/// Available cycle detection strategies.
/// </summary>
public enum CycleDetectionStrategy
{
    /// <summary>
    /// Standard DFS baseline implementation.
    /// </summary>
    Standard,

    /// <summary>
    /// Optimized implementation using Span and stackalloc.
    /// </summary>
    Optimized,

    /// <summary>
    /// Ultra-optimized unsafe implementation with pointer arithmetic.
    /// </summary>
    Unsafe
}
