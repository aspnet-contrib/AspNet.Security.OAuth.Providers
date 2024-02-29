/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Reflection;
using System.Xml.Linq;

namespace AspNet.Security.OAuth;

public static class PackageMetadataTests
{
    private static readonly string? _solutionRoot = typeof(PackageMetadataTests).Assembly
        .GetCustomAttributes<AssemblyMetadataAttribute>()
        .Where((p) => string.Equals("SolutionRoot", p.Key, StringComparison.Ordinal))
        .Select((p) => p.Value)
        .First();

    public static TheoryData<string, string> Projects()
    {
        var testCases = new TheoryData<string, string>();

        foreach (var directory in Directory.EnumerateDirectories(Path.Combine(_solutionRoot!, "src")))
        {
            foreach (var project in Directory.EnumerateFiles(directory, "*.csproj"))
            {
                var projectName = Path.GetFileNameWithoutExtension(project);

                foreach (var propertyName in new[] { "Authors", "Description", "PackageTags" })
                {
                    testCases.Add(projectName, propertyName);
                }
            }
        }

        return testCases;
    }

    [Theory]
    [MemberData(nameof(Projects))]
    public static async Task Project_Has_Expected_Package_Metadata(string projectName, string propertyName)
    {
        // Arrange
        var path = Path.Combine(_solutionRoot!, "src", projectName, projectName) + ".csproj";

        using var stream = File.OpenRead(path);
        XElement project = await XElement.LoadAsync(stream, LoadOptions.None, CancellationToken.None);

        AssertPackageMetadata(project, propertyName);
    }

    private static void AssertPackageMetadata(XElement project, string propertyName)
    {
        var found = false;

        foreach (XElement item in project.Descendants("PropertyGroup").Descendants())
        {
            if (string.Equals(item.Name.LocalName, propertyName, StringComparison.Ordinal))
            {
                item.Value.ShouldNotBeNullOrWhiteSpace($"The {propertyName} MSBuild property has no value.");
                found = true;
                break;
            }
        }

        found.ShouldBeTrue($"The {propertyName} MSBuild property cannot be found.");
    }
}
