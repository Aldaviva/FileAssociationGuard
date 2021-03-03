using System.Collections.Generic;
using System.Linq;

// ReSharper disable InconsistentNaming - library functions that are supposed to look like Linq methods, which user UpperCase naming.

namespace FileAssociationGuard {

    public static class EnumerableExtensions {

        /// <summary>
        ///     Remove null values.
        /// </summary>
        /// <returns>Input enumerable with null values removed.</returns>
        public static IEnumerable<T> Compact<T>(this IEnumerable<T?> source) where T: class {
            return source.Where(item => item != null)!;
        }

        /// <summary>
        ///     Remove null values.
        /// </summary>
        /// <returns>Input enumerable with null values removed.</returns>
        public static IEnumerable<T> Compact<T>(this IEnumerable<T?> source) where T: struct {
            return (IEnumerable<T>) source.Where(item => item != null);
        }

    }

}