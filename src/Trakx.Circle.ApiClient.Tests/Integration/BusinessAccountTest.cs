using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Trakx.Circle.ApiClient.Tests.Integration;

public class BusinessAccountTest: CircleClientTestsBase
{
    private readonly  ICircleBusinessAccountClient _businessAccountClient;
    private readonly ICircleAccountsClient _accountsClient;
    private readonly MockCreator _mockCreator;
    public BusinessAccountTest(CircleApiFixture apiFixture, ITestOutputHelper output) : base(apiFixture, output)
    {
        _accountsClient = ServiceProvider.GetRequiredService<ICircleAccountsClient>();
        _businessAccountClient = ServiceProvider.GetRequiredService<ICircleBusinessAccountClient>();
        _mockCreator = new MockCreator(output);
    }

    [Fact(Skip = "Not working...")]
    public async Task Creating_Signet_Should_Succeed()
    {
        var businesses = await _businessAccountClient.GetSignetBanksAsync();
        businesses.Should().NotBeNull();

        if (businesses.Content.Data.Count >= 3) return;

        var signetBankCreationRequest = _mockCreator.GetSignetWireCreationRequest();
        var business = await _businessAccountClient.CreateSignetBankAsync(signetBankCreationRequest);

        business.Content.Data.TrackingRef.Should().NotBeNullOrEmpty();
        business.StatusCode.Should().Be(StatusCodes.Status200OK);

    }

    [Fact(Skip = "Not working...")]
    public async Task Getting_Signet_Should_Succeed()
    {
        var business = await _businessAccountClient.GetSignetBanksAsync();
        business.Content.Data.Should().NotBeNullOrEmpty();
        business.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact(Skip = "Not working...")]
    public async Task Create_Signet_Bank_Should_Be_Successful()
    {
        var bankRequest = _mockCreator.GetSignetWireCreationRequest();
        Logger.Information("Creating a signet bank account with IdempotencyKey {IdempotencyKey}", bankRequest.IdempotencyKey);

        var result = await _businessAccountClient.CreateSignetBankAsync(bankRequest);

        result.Content.Data.TrackingRef.Should().NotBeNullOrEmpty();
        result.Content.Data.WalletAddress.Should().NotBeNullOrEmpty();
        new HttpResponseMessage((HttpStatusCode)result.StatusCode).IsSuccessStatusCode.Should().BeTrue();
        result.Content.Data.Id.Should().NotBeEmpty();
        Logger.Information("Signet bank account created with trackingRef {TrackingRef}", result.Content.Data.TrackingRef);
    }

    [Fact(Skip = "Not working...")]
    public async Task Create_SilverGate_Bank_Should_Be_Successful()
    {
        var bankRequest = _mockCreator.GetSilverGateSenBankRequest;
        Logger.Information("Creating a silver gate bank account with IdempotencyKey {IdempotencyKey}", bankRequest.IdempotencyKey);

        var result = await _businessAccountClient.CreateSilverGateBankAsync(bankRequest);

        result.Content.Data.TrackingRef.Should().NotBeNullOrEmpty();
        result.Content.Data.Description.Should().NotBeNullOrEmpty();
        result.Content.Data.Currency.Should().NotBeNullOrEmpty();
        result.Content.Data.Id.Should().NotBeEmpty();
        new HttpResponseMessage((HttpStatusCode)result.StatusCode).IsSuccessStatusCode.Should().BeTrue();
        Logger.Information("silver gate bank account created with trackingRef {TrackingRef}", result.Content.Data.TrackingRef);
    }

    [Fact(Skip = "Not working...")]
    public async Task Get_SilverGate_Bank_Should_Be_Successful()
    {
        var result = await _businessAccountClient.GetSilverGateBanksAsync();
        result.Content.Data.Should().NotBeNullOrEmpty();
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact(Skip = "Not working...")]
    public async Task Get_Signet_Bank_Should_Be_Successful()
    {
        var result = await _businessAccountClient.GetSignetBanksAsync();
        result.Content.Data.Should().NotBeNullOrEmpty();
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task Get_Balance_Should_Be_Successful()
    {
        Logger.Information("Getting account balance");
        const int minCount = 0;
        var result = await _accountsClient.GetBalancesAsync();

        var availableBalance = result.Content.Data.Available;

        Logger.Information("{balanceCount} account balance(s) retrieved", availableBalance.Count);
        foreach (var money in availableBalance)
        {
            Logger.Information("{Amount} {Currency}", money.Amount, money.Currency);
        }

        availableBalance.Should().HaveCountGreaterThanOrEqualTo(minCount);
        result.Content.Data.Unsettled.Should().HaveCountGreaterThanOrEqualTo(minCount);
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
    [Fact(Skip = "Payment SilverGate payment not working now")]
    public async Task Creating_SilverGate_Transfer_Should_Succeed()
    {
        var bankRequest = _mockCreator.GetSignetWireCreationRequest();
        Logger.Information("Creating a signet bank account with IdempotencyKey {IdempotencyKey}", bankRequest.IdempotencyKey);

        var result = await _businessAccountClient.CreateSignetBankAsync(bankRequest);

        var request = _mockCreator.GetSilverGateSenBankTransferRequest(result.Content.Data.TrackingRef);
        var transferResponse = await _businessAccountClient.CreateSilverGateMockTransferAsync(request,CancellationToken.None);

        transferResponse.Content.Data.TrackingRef.Should().Be(result.Content.Data.TrackingRef);
        transferResponse.StatusCode.Should().Be(StatusCodes.Status201Created);
    }
}
