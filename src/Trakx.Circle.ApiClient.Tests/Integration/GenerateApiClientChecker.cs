using Trakx.Common.Testing.Documentation.GenerateApiClient;

namespace Trakx.Circle.ApiClient.Tests.Integration;

public class GenerateApiClientChecker : GenerateApiClientCheckerBase
{
    public GenerateApiClientChecker(ITestOutputHelper output)
        : base(output, CreateProjectFileFinder())
    {
    }

    private static IProjectFileFinder CreateProjectFileFinder()
    {
        var objectFromClientAssembly = new CircleApiConfiguration();
        var currentDirectory = Environment.CurrentDirectory;
        return new ProjectFileFinder(objectFromClientAssembly, currentDirectory);
    }
}