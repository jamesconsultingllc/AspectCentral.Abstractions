#if NETSTANDARD2_0
using System;

// ReSharper disable once CheckNamespace
namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property |
                AttributeTargets.ReturnValue)]
internal sealed class NotNullAttribute : Attribute
{
}
#endif
