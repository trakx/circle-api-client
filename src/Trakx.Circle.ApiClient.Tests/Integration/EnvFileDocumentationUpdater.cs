using Trakx.Utils.Testing;
using Xunit.Abstractions;

namespace Trakx.Circle.ApiClient.Tests.Integration
{
    public class EnvFileDocumentationUpdater : Trakx.Utils.Testing.EnvFileDocumentationUpdaterBase
    {
        public EnvFileDocumentationUpdater(ITestOutputHelper output, IReadmeEditor? editor = null) : base(output, editor)
        {
        }
    }
}