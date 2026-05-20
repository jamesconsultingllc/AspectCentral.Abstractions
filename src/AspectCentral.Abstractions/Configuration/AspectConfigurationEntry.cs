using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AspectCentral.Abstractions.Internal;
using JamesConsulting.Reflection;

namespace AspectCentral.Abstractions.Configuration;

/// <summary>
/// Describes a single aspect attached to a service: which aspect type, in what order, and which methods
/// it should intercept. Equality is by <see cref="AspectType" />.
/// </summary>
public class AspectConfigurationEntry : IEqualityComparer<AspectConfigurationEntry?>
{
    private List<MethodInfo> methodsToIntercept;

    /// <summary>Initializes a new instance of the <see cref="AspectConfigurationEntry" /> class.</summary>
    /// <param name="aspectType">The concrete aspect type. Must be a concrete class.</param>
    /// <param name="sortOrder">Sort order used by the runtime to determine aspect application order.</param>
    /// <param name="methodsToIntercept">Methods to intercept on the service interface.</param>
    /// <exception cref="ArgumentNullException"><paramref name="aspectType" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="aspectType" /> is not a concrete class.</exception>
    internal AspectConfigurationEntry(Type aspectType, int sortOrder, params MethodInfo[]? methodsToIntercept)
    {
        Guard.NotNull(aspectType);

        if (!aspectType.IsConcreteClass())
            throw new ArgumentException("Type must be a concrete class", nameof(aspectType));

        AspectType = aspectType;
        SortOrder = sortOrder;
        this.methodsToIntercept = methodsToIntercept is null
            ? new List<MethodInfo>()
            : new List<MethodInfo>(methodsToIntercept);
    }

    /// <summary>Gets the aspect type associated with this entry.</summary>
    public Type AspectType { get; }

    /// <summary>Gets the sort order used to determine aspect application order at runtime.</summary>
    public int SortOrder { get; }

    /// <inheritdoc />
    public virtual bool Equals(AspectConfigurationEntry? x, AspectConfigurationEntry? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.AspectType == y.AspectType;
    }

    /// <inheritdoc />
    public virtual int GetHashCode(AspectConfigurationEntry? obj) =>
        obj?.AspectType != null ? obj.AspectType.GetHashCode() : 0;

    /// <summary>Compares two entries by <see cref="AspectType" />.</summary>
    /// <param name="other">The other entry.</param>
    /// <returns><c>true</c> when both refer to the same aspect type.</returns>
    public bool Equals(AspectConfigurationEntry? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return AspectType == other.AspectType;
    }

    /// <summary>Equality operator.</summary>
    public static bool operator ==(AspectConfigurationEntry? left, AspectConfigurationEntry? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null) return false;
        return right is not null && left.Equals(right);
    }

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(AspectConfigurationEntry? left, AspectConfigurationEntry? right) =>
        !(left == right);

    /// <summary>Adds methods to the set this entry intercepts. The set is de-duplicated.</summary>
    /// <param name="newMethodsToIntercept">Methods to add.</param>
    /// <exception cref="ArgumentNullException"><paramref name="newMethodsToIntercept" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="newMethodsToIntercept" /> is empty.</exception>
    public void AddMethodsToIntercept(params MethodInfo[] newMethodsToIntercept)
    {
        Guard.NotNull(newMethodsToIntercept);
        if (newMethodsToIntercept.Length == 0)
            throw new ArgumentException("Value cannot be an empty collection.", nameof(newMethodsToIntercept));

        methodsToIntercept = methodsToIntercept.Union(newMethodsToIntercept).ToList();
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as AspectConfigurationEntry);

    /// <inheritdoc />
    public override int GetHashCode() => GetHashCode(this);

    /// <summary>Returns a snapshot of the methods this entry intercepts.</summary>
    public List<MethodInfo> GetMethodsToIntercept() => methodsToIntercept.ToList();

    /// <summary>
    /// Indicates whether <paramref name="methodInfo" /> is in this entry's intercept set, without
    /// allocating a defensive copy. Used by <see cref="AspectConfiguration.ShouldIntercept" /> on
    /// the per-invocation interception decision path.
    /// </summary>
    internal bool ContainsMethod(MethodInfo methodInfo) => methodsToIntercept.Contains(methodInfo);

    /// <summary>Removes the supplied methods from this entry's intercept set. Missing entries are silently ignored.</summary>
    /// <param name="methodsToBeRemoved">The methods to remove. A null/empty argument is a no-op.</param>
    public void RemoveMethodsToIntercept(params MethodInfo[]? methodsToBeRemoved)
    {
        if (methodsToBeRemoved is null || methodsToBeRemoved.Length == 0) return;
        methodsToIntercept.RemoveAll(methodsToBeRemoved.Contains);
    }
}
