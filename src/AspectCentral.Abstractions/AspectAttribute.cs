using System;

namespace AspectCentral.Abstractions;

/// <summary>
/// Marks a type as an AspectCentral aspect. Aspects discovered with this attribute in any assembly
/// loaded into the current <see cref="System.AppDomain" /> are auto-registered as singletons by
/// <c>AddAspectSupport</c>, and can be attached to service registrations with <c>AddAspect&lt;T&gt;()</c>.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class AspectAttribute : Attribute
{
}
