﻿using Trakx.Utils.Extensions;
using Xunit.Abstractions;
using Trakx.Utils.Testing;
namespace Trakx.Circle.ApiClient.Tests.Integration;

public class OpenApiGeneratedCodeModifiers : OpenApiGeneratedCodeModifier
{
    public OpenApiGeneratedCodeModifiers(ITestOutputHelper output)
        : base(output)
    {
        var foundRoot = default(DirectoryInfo).TryWalkBackToRepositoryRoot(out var rootDirectory)!; 
        FilePaths.Add(Path.Combine(rootDirectory!.FullName, "src",
            "Trakx.Circle.ApiClient", "ApiClients.cs"));
    }
}