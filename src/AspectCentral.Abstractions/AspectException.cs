using System;

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
    public AspectException(string errorCode, string message)
        : base(message)
    {
        ErrorCode = errorCode;
    }

    /// <summary>Initializes a new instance of the <see cref="AspectException" /> class with an inner exception.</summary>
    /// <param name="errorCode">A stable code from <see cref="AspectErrorCodes" />.</param>
    /// <param name="message">A human-readable description of the failure.</param>
    /// <param name="innerException">The triggering exception.</param>
    public AspectException(string errorCode, string message, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }

    /// <summary>Gets the stable error code that identifies the failure category.</summary>
    public string ErrorCode { get; } = string.Empty;
}
