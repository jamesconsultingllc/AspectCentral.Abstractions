using System;
using AspectCentral.Abstractions.Internal;

namespace AspectCentral.Abstractions;

/// <summary>
/// Domain exception thrown by the AspectCentral abstractions when an operation violates a structural
/// rule (for example, attaching an aspect before any service has been registered). Wraps a stable
/// <see cref="AspectErrorCodes" /> identifier so callers can react programmatically without parsing
/// the message.
/// </summary>
public class AspectException : Exception
{
    /// <summary>Initializes a new instance of the <see cref="AspectException" /> class.</summary>
    /// <param name="errorCode">A stable code from <see cref="AspectErrorCodes" />.</param>
    /// <param name="message">A human-readable description of the failure.</param>
    /// <exception cref="ArgumentNullException">Either argument is <c>null</c>.</exception>
    public AspectException(string errorCode, string message)
        : base(NotNull(message))
    {
        Guard.NotNull(errorCode);
        ErrorCode = errorCode;
    }

    /// <summary>Initializes a new instance of the <see cref="AspectException" /> class with an inner exception.</summary>
    /// <param name="errorCode">A stable code from <see cref="AspectErrorCodes" />.</param>
    /// <param name="message">A human-readable description of the failure.</param>
    /// <param name="innerException">The triggering exception.</param>
    /// <exception cref="ArgumentNullException">Any argument is <c>null</c>.</exception>
    public AspectException(string errorCode, string message, Exception innerException)
        : base(NotNull(message), NotNull(innerException))
    {
        Guard.NotNull(errorCode);
        ErrorCode = errorCode;
    }

    /// <summary>Gets the stable error code that identifies the failure category.</summary>
    public string ErrorCode { get; } = string.Empty;

    // Local helper: validate before the base-ctor call (Guard.NotNull is void) so the resulting
    // exception never carries null state.
    private static T NotNull<T>(T value, [System.Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : class
    {
        if (value is null) throw new ArgumentNullException(paramName);
        return value;
    }
}
