namespace AspectCentral.Abstractions;

/// <summary>
/// Stable error codes produced by <see cref="AspectException" />. Codes are deliberately stable across
/// versions so that downstream tooling can match on them without parsing localized messages.
/// </summary>
public static class AspectErrorCodes
{
    /// <summary>
    /// An aspect was added before any service had been registered on the builder. Add a service first via
    /// <c>AddScoped</c>/<c>AddTransient</c>/<c>AddSingleton</c>, then attach aspects.
    /// </summary>
    public const string NoServiceRegisteredForAspect = "AC001";

    /// <summary>
    /// The supplied aspect type does not satisfy the structural requirements (must be a concrete class
    /// and, for the public extension entry points, decorated with <see cref="AspectAttribute" />).
    /// </summary>
    public const string InvalidAspectType = "AC002";

    /// <summary>
    /// The supplied service-implementation pair is invalid: the implementation is not a concrete class
    /// or is not assignable to the service interface.
    /// </summary>
    public const string InvalidServiceRegistration = "AC003";

    /// <summary>
    /// The supplied <see cref="System.Type" /> argument is not assignable to <see cref="IAspectRegistrationBuilder" />.
    /// </summary>
    public const string InvalidRegistrationBuilderType = "AC004";
}
