using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Circle.ApiClient.Tests.Integration;

public class BusinessAccountTest: CircleClientTestsBase
{
    private readonly  IBusinessAccountClient _businessAccountClient;
    private readonly MockCreators _mockCreator;
    public BusinessAccountTest(CircleApiFixture apiFixture, ITestOutputHelper output) : base(apiFixture, output)
    {
        _businessAccountClient = ServiceProvider.GetRequiredService<IBusinessAccountClient>();
        _mockCreator = new MockCreators(output);
    }
    
    [Fact]
    public async Task Creating_Signet_Should_Succeed()
    {
        var businesses = await _businessAccountClient.GetSignetBanksAsync();
        if (businesses.Result.Data.Count < 3)
        {
            var business = await _businessAccountClient.CreateSignetBankAsync(new SignetBankCreationRequest
            {
                IdempotencyKey   = Guid.NewGuid().ToString(),
                WalletAddress = _mockCreator.GetEthereumAddress(),
            });
        
            business.Result.Data.TrackingRef.Should().NotBeNullOrEmpty();
            business.StatusCode.Should().Be(200);
        }
        
        businesses.Should().NotBeNull();

    }
    [Fact]
    public async Task Getting_Signet_Should_Succeed()
    {
        var business = await _businessAccountClient.GetSignetBanksAsync();
        
        business.Result.Data.Should().NotBeNullOrEmpty();
        business.StatusCode.Should().Be(200);
    }
}