# AspectCentral.Abstractions

[![CI](https://github.com/jamesconsultingllc/AspectCentral.Abstractions/actions/workflows/ci.yml/badge.svg?branch=master)](https://github.com/jamesconsultingllc/AspectCentral.Abstractions/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/AspectCentral.Abstractions.svg)](https://www.nuget.org/packages/AspectCentral.Abstractions/)
[![Downloads](https://img.shields.io/nuget/dt/AspectCentral.Abstractions.svg)](https://www.nuget.org/packages/AspectCentral.Abstractions/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

Common AOP (aspect-oriented programming) abstractions for the AspectCentral family. Defines the
`IAspectRegistrationBuilder`, the `[Aspect]` attribute, and `IServiceCollection` extensions that
concrete runtime implementations (DispatchProxy, Castle DynamicProxy, …) plug into.

## Target frameworks

`netstandard2.0` · `netstandard2.1` · `net9.0` · `net10.0`

## Install

```sh
dotnet add package AspectCentral.Abstractions
```

You also need one of the runtime implementations:

| Runtime | Backed by | Package |
|---------|-----------|---------|
| [AspectCentral.DispatchProxy](https://github.com/jamesconsultingllc/AspectCentral.DispatchProxy) | `System.Reflection.DispatchProxy` | `AspectCentral.DispatchProxy` |
| [AspectCentral.Interceptors](https://github.com/jamesconsultingllc/AspectCentral.Interceptors) | Castle DynamicProxy `IInterceptor` | `AspectCentral.Interceptors` |

## Quick start

```csharp
using AspectCentral.Abstractions;
using Microsoft.Extensions.DependencyInjection;

// 1. Mark your aspect with [Aspect].
[Aspect]
public sealed class LoggingAspect
{
    public void OnBefore(AspectContext ctx) { /* … */ }
    public void OnAfter(AspectContext ctx)  { /* … */ }
}

// 2. Wire AspectCentral into your DI container, using a concrete runtime
//    (TAspectRegistrationBuilder = e.g. DispatchProxyAspectRegistrationBuilder).
var services = new ServiceCollection();

services
    .AddAspectSupport(typeof(DispatchProxyAspectRegistrationBuilder))
    .AddScoped<IGreeter, Greeter>()    // register the service
    .AddAspect<LoggingAspect>();       // attach the aspect to the most-recently-added service

var provider = services.BuildServiceProvider();
var greeter  = provider.GetRequiredService<IGreeter>(); // returns a proxy
```

## Key surface

| Type | Role |
|------|------|
| `AspectAttribute` | Marks a type as an aspect. Auto-registered as a singleton by `AddAspectSupport`. |
| `IAspectRegistrationBuilder` | Builder contract. Concrete runtimes implement `InvokeCreateFactory` to produce proxies. |
| `AspectRegistrationBuilder` | Abstract base with the shared bookkeeping (service add, aspect attach). |
| `AspectRegistrationBuilderExtensions` | `AddScoped` / `AddTransient` / `AddSingleton` / `AddAspect<T>()` helpers. |
| `AspectCentralServiceCollectionExtensions` | `AddAspectSupport` entry point. Lives in the `Microsoft.Extensions.DependencyInjection` namespace. |
| `AspectContext` | Per-invocation context: target method, args, return value, `InvokeMethod` short-circuit flag. |
| `AspectException` + `AspectErrorCodes` | Domain exception with stable codes (`AC001`–`AC004`). |
| `AspectCentralDiagnostics` | Well-known `ActivitySourceName` / `MeterName` for downstream runtimes. |

## Versioning

Built with [Nerdbank.GitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning). Version flows from `src/version.json`
plus git height. Pre-release builds publish as `2.0.0-alpha.{height}` on feature branches and
`2.0.0-rc.{height}` on `release/*` branches; the final tag publishes as `2.0.0`.

## Breaking changes in 2.0.0

| Area | Change |
|------|--------|
| Naming | `IServiceCollectionExtensions` → `AspectCentralServiceCollectionExtensions` (now in `Microsoft.Extensions.DependencyInjection` namespace). |
| Naming | `IAspectRegistrationBuilderExtensions` → `AspectRegistrationBuilderExtensions`. |
| Errors | `AddAspect` before any service is registered now throws `AspectException(AspectErrorCodes.NoServiceRegisteredForAspect)` instead of `InvalidOperationException`. |
| Errors | `AddAspectSupport(Type)` with a non-`IAspectRegistrationBuilder` argument now throws `AspectException(AspectErrorCodes.InvalidRegistrationBuilderType)` instead of `ArgumentException`. |
| TFMs | Drops `net5.0`; adds `netstandard2.0`, `net9.0`, `net10.0`. |
| Deps | `Microsoft.Extensions.DependencyInjection.Abstractions` 5.0.0 → 10.0.0. |

## Contributing

See [`AGENTS.md`](AGENTS.md) for the universal contribution rules (vertical slices, BDD/TDD,
security, observability) and [`src/AGENTS.md`](src/AGENTS.md) for the library-specific checklist.
GitFlow: branch off `develop` (`feature/…`), open a PR into `develop`. `master` is the stable
publishing branch.

## License

MIT — see [`LICENSE`](LICENSE).
