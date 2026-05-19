using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AspectCentral.Abstractions.Internal;

/// <summary>
/// Internal argument-validation helpers. Polyfills <see cref="ArgumentNullException.ThrowIfNull(object?, string?)" />
/// on target frameworks that lack it (netstandard2.0 / netstandard2.1).
/// </summary>
internal static class Guard
{
    /// <summary>Throws <see cref="ArgumentNullException" /> when <paramref name="value" /> is <c>null</c>.</summary>
    public static void NotNull<T>(
        [NotNull] T? value,
        [CallerArgumentExpression("value")] string? name = null)
        where T : class
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(value, name);
#else
        if (value is null) throw new ArgumentNullException(name);
#endif
    }
}


