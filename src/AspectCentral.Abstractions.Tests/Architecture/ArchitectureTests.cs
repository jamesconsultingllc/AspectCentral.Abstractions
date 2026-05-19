using System.Reflection;
using NetArchTest.Rules;
using Xunit;

namespace AspectCentral.Abstractions.Tests.Architecture;

/// <summary>
/// Architecture rules that protect AspectCentral.Abstractions structural invariants.
/// These run as ordinary unit tests so a violation breaks the build via CI.
/// </summary>
public class ArchitectureTests
{
    private static readonly Assembly TargetAssembly = typeof(IAspectRegistrationBuilder).Assembly;

    [Fact]
    public void InternalNamespace_IsNotPublic()
    {
        var result = Types.InAssembly(TargetAssembly)
            .That()
            .ResideInNamespace("AspectCentral.Abstractions.Internal")
            .Should()
            .NotBePublic()
            .GetResult();

        Assert.True(result.IsSuccessful,
            "Types under Internal/ must remain internal: " + Join(result.FailingTypeNames));
    }

    [Fact]
    public void PublicTypes_DoNotResideInInternalNamespace()
    {
        var result = Types.InAssembly(TargetAssembly)
            .That()
            .ArePublic()
            .Should()
            .NotResideInNamespace("AspectCentral.Abstractions.Internal")
            .GetResult();

        Assert.True(result.IsSuccessful,
            "Public types must not live under Internal/: " + Join(result.FailingTypeNames));
    }

    [Fact]
    public void ConfigurationNamespace_DoesNotDependOnDependencyInjectionNamespace()
    {
        // Configuration types describe what to wrap, not how to wire DI; they must stay free
        // of the IServiceCollection registration entry point so consumers can use Configuration
        // types without pulling in MEDI extension wiring.
        var result = Types.InAssembly(TargetAssembly)
            .That()
            .ResideInNamespace("AspectCentral.Abstractions.Configuration")
            .ShouldNot()
            .HaveDependencyOn("AspectCentral.Abstractions.DependencyInjection")
            .GetResult();

        Assert.True(result.IsSuccessful,
            "Configuration types must not depend on DependencyInjection namespace: " + Join(result.FailingTypeNames));
    }

    [Fact]
    public void Exceptions_EndWithExceptionSuffix()
    {
        var result = Types.InAssembly(TargetAssembly)
            .That()
            .Inherit(typeof(System.Exception))
            .And()
            .ArePublic()
            .Should()
            .HaveNameEndingWith("Exception")
            .GetResult();

        Assert.True(result.IsSuccessful,
            "Public exception types must end with 'Exception': " + Join(result.FailingTypeNames));
    }

    private static string Join(System.Collections.Generic.IEnumerable<string>? names) =>
        names is null ? "(none reported)" : string.Join(", ", names);
}
