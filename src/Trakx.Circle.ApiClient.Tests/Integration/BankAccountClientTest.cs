using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Circle.ApiClient.Tests.Integration;

public class BankAccountClientTest : CircleClientTestsBase
{
    private readonly IBankAccountsClient _bankAccountsClient;
    private readonly IPaymentsClient _paymentsClient;
    private readonly MockCreator _mockCreator;
    public BankAccountClientTest(CircleApiFixture apiFixture, ITestOutputHelper output) : base(apiFixture, output)
    {
        _bankAccountsClient = ServiceProvider.GetRequiredService<IBankAccountsClient>();
        _paymentsClient = ServiceProvider.GetRequiredService<IPaymentsClient>();
        _mockCreator = new MockCreator(output);
    }

    [Fact]
    public async Task Create_Us_Bank_Account_Should_be_Successful()
    {
        var bankRequest = _mockCreator.WireCreationRequestUs();

        var bankCreated =  await _bankAccountsClient.CreateWireBankAccountAsync(bankRequest);

        bankCreated.Should().NotBeNull();
        bankCreated?.Result.Data.Id.Should().NotBeEmpty();
        int[] expectedCode = new[] { 200, 201 };
        Assert.Single(expectedCode, bankCreated?.StatusCode);
    }
    
    [Fact]
    public async Task Get_Us_Bank_Account_Should_be_Successful()
    {
        var bankRequest = _mockCreator.WireCreationRequestUs();
        var bank =  await _bankAccountsClient.CreateWireBankAccountAsync(bankRequest);
        var id = bank.Result.Data.Id;
        var result =  await _bankAccountsClient.GetWireBankAccountAsync(id);

        result.Should().NotBeNull();
        result?.Result.Data.Id.Should().Be(id);
        Assert.Equal(200,result?.StatusCode);
    }

    [Fact]
    public async Task Create_Mock_Wire_Payment_Should_Be_Initialize_Successful()
    {
        var bankRequest = _mockCreator.WireCreationRequestUs();
        var bank =  await _bankAccountsClient.CreateWireBankAccountAsync(bankRequest);
        var id = bank.Result.Data.Id;
        var result =  await _bankAccountsClient.GetWireBankAccountAsync(id);
        var wireRequest = new MockWirePaymentRequest
        {
            Amount = new Money
            {
                Amount = $"{_mockCreator.GetDecimals()}.00",
                Currency = "USD"
            },
            TrackingRef = result.Result.Data.TrackingRef
        };
        
        // act
        var wirePayment = await _paymentsClient.CreateWirePaymentAsync(wireRequest);
        
        //assert
        wirePayment.Should().NotBeNull();
        Assert.Equal(201, wirePayment.StatusCode);
    }
    
    
}