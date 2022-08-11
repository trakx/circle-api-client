using Xunit.Abstractions;
using Trakx.Utils.Testing.ReadmeUpdater;
namespace Trakx.Circle.ApiClient.Tests.Integration;

public class ReadmeDocumentationUpdaters : ReadmeDocumentationUpdaterBase
{
    public ReadmeDocumentationUpdaters(ITestOutputHelper output, int maxRecursions = 1, bool askForEnvFileSection = false)
        : base(output, maxRecursions, askForEnvFileSection)
    {
    }
}
