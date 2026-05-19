using System;
using System.Runtime.Serialization;

namespace AspectCentral.Abstractions;

/// <summary>
/// Domain exception thrown by the AspectCentral abstractions when an operation violates a structural
/// rule (for example, attaching an aspect before any service has been registered). Wraps a stable
/// <see cref="AspectErrorCodes" /> identifier so callers can react programmatically without parsing
/// the message.
/// </summary>
[Serializable]
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

#if !NET8_0_OR_GREATER
    /// <summary>Serialization constructor.</summary>
    protected AspectException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        ErrorCode = info.GetString(nameof(ErrorCode)) ?? string.Empty;
    }
#endif

    /// <summary>Gets the stable error code that identifies the failure category.</summary>
    public string ErrorCode { get; } = string.Empty;

#if !NET8_0_OR_GREATER
    /// <inheritdoc />
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(ErrorCode), ErrorCode);
    }
#endif
}
