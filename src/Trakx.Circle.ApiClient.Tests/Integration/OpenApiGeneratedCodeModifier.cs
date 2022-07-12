using Trakx.Utils.Extensions;
using Xunit.Abstractions;

namespace Trakx.Circle.ApiClient.Tests.Integration;

public class OpenApiGeneratedCodeModifier : Trakx.Utils.Testing.OpenApiGeneratedCodeModifier
{
    public OpenApiGeneratedCodeModifier(ITestOutputHelper output)
        : base(output)
    {
        var foundRoot = default(DirectoryInfo).TryWalkBackToRepositoryRoot(out var rootDirectory)!; 
        FilePaths.Add(Path.Combine(rootDirectory!.FullName, "src",
            "Trakx.Circle.ApiClient", "ApiClients.cs"));
    }
}