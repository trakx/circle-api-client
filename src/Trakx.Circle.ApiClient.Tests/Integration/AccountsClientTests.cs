using Microsoft.Extensions.DependencyInjection;

namespace Trakx.Circle.ApiClient.Tests.Integration;

public sealed class AccountsClientTests : CircleClientTestsBase
{
    private readonly ICircleAccountsClient _accountsClient;
    public AccountsClientTests(CircleApiFixture apiFixture, ITestOutputHelper output) : base(apiFixture, output)
    {
        _accountsClient = ServiceProvider.GetRequiredService<ICircleAccountsClient>();
    }

    [Fact]
    public async Task GetBalance_should_work()
    {
        var balances = await _accountsClient.GetBalancesAsync();
        balances.Should().NotBeNull();
        balances.Content.Data.Available.Should().NotBeNull();
        balances.Content.Data.Unsettled.Should().NotBeNull();
    }
}
