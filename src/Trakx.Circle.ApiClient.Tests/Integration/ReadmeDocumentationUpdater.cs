using Xunit.Abstractions;

namespace Trakx.Circle.ApiClient.Tests.Integration;

public class ReadmeDocumentationUpdater : Trakx.Utils.Testing.ReadmeUpdater.ReadmeDocumentationUpdaterBase
{
    public ReadmeDocumentationUpdater(ITestOutputHelper output, int maxRecursions = 1, bool askForEnvFileSection = false) : base(output, maxRecursions, askForEnvFileSection)
    {
    }
}
