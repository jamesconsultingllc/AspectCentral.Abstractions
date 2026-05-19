namespace AspectCentral.Abstractions.Diagnostics;

/// <summary>
/// Well-known telemetry source names for the AspectCentral family. Downstream runtime libraries
/// (Castle DynamicProxy, DispatchProxy, etc.) should create their <see cref="System.Diagnostics.ActivitySource" />
/// and <see cref="System.Diagnostics.Metrics.Meter" /> using these names so consumers can subscribe to
/// a single, stable name across implementations.
/// </summary>
public static class AspectCentralDiagnostics
{
    /// <summary>The well-known <see cref="System.Diagnostics.ActivitySource" /> name for AspectCentral runtimes.</summary>
    public const string ActivitySourceName = "AspectCentral";

    /// <summary>The well-known <see cref="System.Diagnostics.Metrics.Meter" /> name for AspectCentral runtimes.</summary>
    public const string MeterName = "AspectCentral";
}
