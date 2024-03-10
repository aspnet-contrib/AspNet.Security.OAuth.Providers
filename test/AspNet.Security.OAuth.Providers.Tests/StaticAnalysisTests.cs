/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Reflection;
using System.Runtime.Loader;

namespace AspNet.Security.OAuth;

public static class StaticAnalysisTests
{
#if JETBRAINS_ANNOTATIONS
    [Fact]
#endif
    public static void Publicly_Visible_Parameters_Have_Null_Annotations()
    {
        // Arrange
        var assemblyNames = GetProviderAssemblyNames();
        var types = GetPublicTypes(assemblyNames);
        var nullabilityContext = new NullabilityInfoContext();

        var testedMethods = 0;

        // Act
        foreach (var type in types)
        {
            var methods = GetPublicOrProtectedConstructorsOrMethods(type);

            foreach (var method in methods)
            {
                var parameters = method.GetParameters();

                foreach (var parameter in parameters)
                {
                    var hasNullabilityAnnotation = parameter
                        .GetCustomAttributes()
                        .Any((p) => p.GetType().FullName == "JetBrains.Annotations.CanBeNullAttribute" ||
                                    p.GetType().FullName == "JetBrains.Annotations.NotNullAttribute");

                    var nullabilityInfo = nullabilityContext.Create(parameter);

                    if (nullabilityInfo.WriteState == NullabilityState.Nullable)
                    {
                        continue;
                    }

                    // Assert
                    hasNullabilityAnnotation.ShouldBeTrue(
                        $"The {parameter.Name} parameter of {type.Name}.{method.Name} does not have a [NotNull] or [CanBeNull] annotation.");

                    testedMethods++;
                }
            }
        }

        testedMethods.ShouldBeGreaterThan(0);
    }

    private static AssemblyName[] GetProviderAssemblyNames()
    {
        var thisType = typeof(StaticAnalysisTests);

        var assemblies = thisType.Assembly
            .GetReferencedAssemblies()
            .Where((p) => p.FullName.StartsWith(thisType.Namespace + ".", StringComparison.Ordinal))
            .ToArray();

        assemblies.ShouldNotBeEmpty();

        return assemblies;
    }

    private static Type[] GetPublicTypes(IEnumerable<AssemblyName> assemblyNames)
    {
        var types = assemblyNames
            .Select(AssemblyLoadContext.Default.LoadFromAssemblyName)
            .SelectMany((p) => p.GetTypes())
            .Where((p) => p.IsPublic || p.IsNestedPublic)
            .ToArray();

        types.ShouldNotBeEmpty();

        return types;
    }

    private static MethodBase[] GetPublicOrProtectedConstructorsOrMethods(Type type)
    {
        var constructors = type
            .GetConstructors(BindingFlags.Public | BindingFlags.NonPublic)
            .Select((p) => (MethodBase)p)
            .ToArray();

        var methods = type
            .GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
            .ToArray();

        return methods
            .Concat(constructors)
            .Where((p) => p.IsPublic || p.IsFamily) // public or protected
            .Where((p) => !p.IsAbstract)
            .Where((p) => !p.IsSpecialName) // No property get/set or event add/remove methods
            .ToArray();
    }
}
