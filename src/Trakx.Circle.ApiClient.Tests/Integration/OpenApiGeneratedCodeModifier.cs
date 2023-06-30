using Trakx.Common.Infrastructure.Environment.Env;
using Xunit.Abstractions;
namespace Trakx.Circle.ApiClient.Tests.Integration;

public class OpenApiGeneratedCodeModifiers : Common.Testing.Documentation.OpenApiGeneratedCodeModifier
{
    public OpenApiGeneratedCodeModifiers(ITestOutputHelper output)
        : base(output)
    {
        var foundRoot = default(DirectoryInfo).TryWalkBackToRepositoryRoot(out var rootDirectory)!; 
        FilePaths.Add(Path.Combine(rootDirectory!.FullName, "src",
            "Trakx.Circle.ApiClient", "ApiClients.cs"));
    }
}