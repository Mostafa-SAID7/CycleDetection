using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace CycleDetection.SourceGenerators;

[Generator]
public class GridFlatteningGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Simple implementation: generate a static extension class with FlattenFast
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "GridExtensions.Generated.cs",
            SourceText.From(@"
namespace CycleDetection.Extensions
{
    public static partial class GridExtensions
    {
        /// <summary>
        /// Flattens a 2D jagged array into a 1D array using optimized generated code.
        /// </summary>
        public static char[] FlattenFast(this char[][] grid)
        {
            if (grid == null) return System.Array.Empty<char>();
            
            int rows = grid.Length;
            if (rows == 0) return System.Array.Empty<char>();
            
            int cols = grid[0].Length;
            char[] flattened = new char[rows * cols];
            
            for (int i = 0; i < rows; i++)
            {
                System.Array.Copy(grid[i], 0, flattened, i * cols, cols);
            }
            
            return flattened;
        }
    }
}
", Encoding.UTF8)));
    }
}
