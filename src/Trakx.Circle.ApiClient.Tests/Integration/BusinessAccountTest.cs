using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Circle.ApiClient.Tests.Integration;

public class BusinessAccountTest: CircleClientTestsBase
{
    private readonly  IBusinessAccountClient _businessAccountClient;
    private readonly IAccountsClient _accountsClient;
    private readonly MockCreator _mockCreator;
    public BusinessAccountTest(CircleApiFixture apiFixture, ITestOutputHelper output) : base(apiFixture, output)
    {
        _accountsClient = ServiceProvider.GetRequiredService<IAccountsClient>();
        _businessAccountClient = ServiceProvider.GetRequiredService<IBusinessAccountClient>();
        _mockCreator = new MockCreator(output);
    }

    [Fact]
    public async Task Creating_Signet_Should_Succeed()
    {
        var businesses = await _businessAccountClient.GetSignetBanksAsync();
        businesses.Should().NotBeNull();

        if (businesses.Result.Data.Count >= 3) return;

        var signetBankCreationRequest = _mockCreator.GetSignetWireCreationRequest();
        var business = await _businessAccountClient.CreateSignetBankAsync(signetBankCreationRequest);

        business.Result.Data.TrackingRef.Should().NotBeNullOrEmpty();
        business.StatusCode.Should().Be(StatusCodes.Status200OK);

    }
    [Fact]
    public async Task Getting_Signet_Should_Succeed()
    {
        var business = await _businessAccountClient.GetSignetBanksAsync();

        business.Result.Data.Should().NotBeNullOrEmpty();
        business.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task Create_Signet_Bank_Should_Be_Successful()
    {
        var bankRequest = _mockCreator.GetSignetWireCreationRequest();
        Logger.Information("Creating a signet bank account with IdempotencyKey {IdempotencyKey}", bankRequest.IdempotencyKey);

        var result = await _businessAccountClient.CreateSignetBankAsync(bankRequest);

        result.Result.Data.TrackingRef.Should().NotBeNullOrEmpty();
        result.Result.Data.WalletAddress.Should().NotBeNullOrEmpty();
        new HttpResponseMessage((HttpStatusCode)result.StatusCode).IsSuccessStatusCode.Should().BeTrue();
        result.Result.Data.Id.Should().NotBeEmpty();
        Logger.Information("Signet bank account created with trackingRef {TrackingRef}", result.Result.Data.TrackingRef);
    }

    [Fact]
    public async Task Get_Signet_Bank_Should_Be_Successful()
    {
        var result = await _businessAccountClient.GetSignetBanksAsync();

        result.Result.Data.Should().NotBeNullOrEmpty();

        result.StatusCode.Should().Be(StatusCodes.Status200OK);

    }

    [Fact]
    public async Task Get_Balance_Should_Be_Successful()
    {
        Logger.Information("Getting account balance");
        const int minCount = 0;
        var result = await _accountsClient.GetBalancesAsync();

        var availableBalance = result.Result.Data.Available;

        Logger.Information("{balanceCount} account balance(s) retrieved", availableBalance.Count);
        foreach (var money in availableBalance)
        {
            Logger.Information("{Amount} {Currency}", money.Amount, money.Currency);
        }

        availableBalance.Should().HaveCountGreaterOrEqualTo(minCount);
        result.Result.Data.Unsettled.Should().HaveCountGreaterOrEqualTo(minCount);
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

}
