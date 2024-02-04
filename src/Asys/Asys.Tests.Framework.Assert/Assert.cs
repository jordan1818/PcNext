using Asys.Tests.Framework.Asserts.Default;

namespace Asys.Tests.Framework.Asserts;

/// <summary>
/// A static assertion set
/// for <see cref="IAssert"/> implementation.
/// </summary>
public static partial class Assert
{
    /// <summary>
    /// The implemented <see cref="IAssert"/>
    /// for general assertions.
    /// </summary>
    public static IAssert Asserter { get; } = GetAsserter();

    /// <summary>
    /// Gets an instance of <see cref="IAssert"/> from any attached assembly during runtime.
    /// </summary>
    /// <returns>An implemented instance of <see cref="IAssert"/>.</returns>
    /// <exception cref="InvalidOperationException">When attached assembly encounters problems when instanciating a instance of <see cref="IAssert"/>.</exception>
    private static IAssert GetAsserter()
    {
        // Retrieves all the current runtime assemblies.
        var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        
        // Get all assemblies that implemented IAssert
        // and is not the default.
        var assmbliesWithAsserters = allAssemblies.Where(a => 
            a.GetTypes()
                .Any(t => t.GetInterfaces().Any(i => i == typeof(IAssert)) && 
                            t != typeof(DefaultAssert)));
        
        // If there are assemblies with implemented IAssert
        // Attempt to create them instead.
        if (assmbliesWithAsserters.Any())
        {
            // If there is more than one assemblies
            // with implemented IAssert, fail here.
            if (assmbliesWithAsserters.Count() > 1)
            {
                throw new InvalidOperationException($"More than one assembly was loaded with interface '{nameof(IAssert)}'. Could not determine which to choose. The assemblies '{string.Join(", ", assmbliesWithAsserters)}'");
            }

            // Get the only assembly and the type tHat implemented IAssert.
            var assembly = assmbliesWithAsserters.ElementAt(0);
            var type = assembly.GetTypes().Single(t => t.GetInterfaces().Any(i => i == typeof(IAssert)));

            // Attempt to instanciate the instance of
            // the implemented type of IAssert
            // and validate.
            var asserter = Activator.CreateInstance(type) as IAssert;
            if (asserter is null)
            {
                throw new InvalidOperationException($"The instance of type '{type.FullName}' was null from assembly '{assembly.FullName}'.");
            }

            return asserter;
        }

        return new DefaultAssert();
    }
}
