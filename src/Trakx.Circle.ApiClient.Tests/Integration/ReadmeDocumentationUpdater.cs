using Trakx.Common.Testing.Documentation.ReadmeUpdater;

namespace Trakx.Circle.ApiClient.Tests.Integration;

public class ReadmeDocumentationUpdaters : ReadmeDocumentationUpdaterBase
{
    public ReadmeDocumentationUpdaters(ITestOutputHelper output, bool askForEnvFileSection = false)
        : base(output, askForEnvFileSection)
    {
    }
}