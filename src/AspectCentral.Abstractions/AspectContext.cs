using System.Reflection;
using AspectCentral.Abstractions.Internal;
using JamesConsulting.Reflection;

namespace AspectCentral.Abstractions;

/// <summary>
/// Captures the per-invocation state passed between an aspect interceptor and the target method:
/// the method being intercepted, the argument values, whether the underlying method should still
/// execute, and the return value. Mutable so an aspect can short-circuit execution
/// (<see cref="InvokeMethod" /> = false) or rewrite the result (<see cref="ReturnValue" />).
/// </summary>
public class AspectContext
{
    /// <summary>Initializes a new instance of the <see cref="AspectContext" /> class.</summary>
    /// <param name="targetMethod">The method being intercepted. Required.</param>
    /// <param name="parameterValues">The argument values supplied to the invocation. May be <c>null</c> for parameterless methods.</param>
    /// <exception cref="System.ArgumentNullException"><paramref name="targetMethod" /> is <c>null</c>.</exception>
    public AspectContext(MethodInfo targetMethod, object[]? parameterValues)
    {
        Guard.NotNull(targetMethod);
        TargetMethod = targetMethod;
        ParameterValues = parameterValues;
        SetMethodType();
    }

    /// <summary>Gets or sets an aspect-defined string describing the current invocation (e.g. for logging).</summary>
    public string? InvocationString { get; set; }

    /// <summary>Gets or sets a value indicating whether the underlying method should still be invoked. Defaults to <c>true</c>.</summary>
    public bool InvokeMethod { get; set; } = true;

    /// <summary>Gets the classification of <see cref="TargetMethod" /> (sync/async × action/function).</summary>
    public MethodTypeOptions MethodType { get; private set; }

    /// <summary>Gets the argument values supplied to the invocation.</summary>
    public object[]? ParameterValues { get; }

    /// <summary>Gets or sets the value to return to the caller. Aspects may rewrite this before or after invocation.</summary>
    public object? ReturnValue { get; set; }

    /// <summary>Gets the method being intercepted (as declared on the service interface).</summary>
    public MethodInfo TargetMethod { get; }

    /// <summary>Gets or sets the concrete implementation method that backs <see cref="TargetMethod" /> on the target instance.</summary>
    public MethodInfo? InstanceMethod { get; set; }

    private void SetMethodType()
    {
        if (TargetMethod.IsAsync())
            MethodType = TargetMethod.IsAsyncWithResult()
                ? MethodTypeOptions.AsyncFunction
                : MethodTypeOptions.AsyncAction;
        else
            MethodType = TargetMethod.HasReturnValue()
                ? MethodTypeOptions.SyncFunction
                : MethodTypeOptions.SyncAction;
    }
}
