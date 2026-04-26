using System;
using CycleDetection.Abstractions;
using CycleDetection.Core;
using CycleDetection.Optimizations;
using Microsoft.Extensions.DependencyInjection;

namespace CycleDetection.DependencyInjection;

/// <summary>
/// Dependency injection extensions for cycle detection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds cycle detection services to the dependency injection container.
    /// </summary>
    public static IServiceCollection AddCycleDetection(
        this IServiceCollection services,
        Action<CycleDetectionOptions>? configure = null)
    {
        var options = new CycleDetectionOptions();
        configure?.Invoke(options);

        services.AddSingleton(options);

        services.AddSingleton<ICycleDetector>(provider =>
        {
            var opts = provider.GetRequiredService<CycleDetectionOptions>();
            return opts.Strategy switch
            {
                CycleDetectionStrategy.Standard => new StandardDfsDetector(),
                CycleDetectionStrategy.Optimized => new OptimizedDfsDetector(),
                CycleDetectionStrategy.Unsafe => new UnsafeDfsDetector(),
                _ => new OptimizedDfsDetector()
            };
        });

        return services;
    }

    /// <summary>
    /// Configures cycle detection to use the standard implementation.
    /// </summary>
    public static CycleDetectionOptions UseStandard(this CycleDetectionOptions options)
    {
        options.Strategy = CycleDetectionStrategy.Standard;
        return options;
    }

    /// <summary>
    /// Configures cycle detection to use the optimized implementation.
    /// </summary>
    public static CycleDetectionOptions UseOptimized(this CycleDetectionOptions options)
    {
        options.Strategy = CycleDetectionStrategy.Optimized;
        return options;
    }

    /// <summary>
    /// Configures cycle detection to use the unsafe implementation.
    /// </summary>
    public static CycleDetectionOptions UseUnsafe(this CycleDetectionOptions options)
    {
        options.Strategy = CycleDetectionStrategy.Unsafe;
        return options;
    }
}
