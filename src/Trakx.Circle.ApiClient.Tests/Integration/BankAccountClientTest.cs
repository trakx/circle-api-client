using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Circle.ApiClient.Tests.Integration;

/// <summary>
/// Bank account client test
/// <see cref="BankAccountClientTest"/>
/// </summary>
public class BankAccountClientTest : CircleClientTestsBase
{
    private readonly IBankAccountsClient _bankAccountsClient;
    private readonly IPaymentsClient _paymentsClient;
    private readonly MockCreators _mockCreator;
    /// <summary>
    /// constructor for <see cref="BankAccountClientTest"/>
    /// </summary>
    /// <param name="apiFixture"></param>
    /// <param name="output"></param>
    public BankAccountClientTest(CircleApiFixture apiFixture, ITestOutputHelper output) : base(apiFixture, output)
    {
        _bankAccountsClient = ServiceProvider.GetRequiredService<IBankAccountsClient>();
        _paymentsClient = ServiceProvider.GetRequiredService<IPaymentsClient>();
        _mockCreator = new MockCreators(output);
        Logger.Information($"Instantiating  {nameof(BankAccountClientTest)}");
    }

    /// <summary>
    /// creating US bank account with valid payload should be successful
    /// <remarks>
    /// The documentation state that the response code should be 201, but the sandbox response returned 200
    /// either 200 or 201 should be valid
    /// </remarks>
    /// </summary>
    [Fact]
    public async Task Create_Us_Bank_Account_Should_be_Successful()
    {
        Logger.Information("Running test for creating new Us Bank");
        var bankRequest = _mockCreator.WireCreationRequestUs();

        var bankCreated =  await _bankAccountsClient.CreateWireBankAccountAsync(bankRequest);

        bankCreated.Should().NotBeNull();
        bankCreated?.Result.Data.Id.Should().NotBeEmpty();
        int[] expectedCode = { 200, 201 };
        Logger.Information($"Us bank was created successfully with {bankCreated?.Result.Data.Id}");
        Assert.Single(expectedCode, bankCreated?.StatusCode);
    }
    
    /// <summary>
    /// Retrieving US bank account with valid id
    /// </summary>
    [Fact]
    public async Task Get_Us_Bank_Account_Should_be_Successful()
    {
        var bankRequest = _mockCreator.WireCreationRequestUs();
        var bank =  await _bankAccountsClient.CreateWireBankAccountAsync(bankRequest);
        var id = bank.Result.Data.Id;
        Logger.Information($"Retrieving back account for wire transfer with id {id}");
        var result =  await _bankAccountsClient.GetWireBankAccountAsync(id);

        result.Should().NotBeNull();
        result?.Result.Data.Id.Should().Be(id);
        Logger.Information($"Bank account returned with status code {result?.StatusCode}");
        Assert.Equal(200,result?.StatusCode);
    }

    /// <summary>
    /// Test to initiate a mock wire payment that mimics the behavior of funds sent through the bank (wire) account linked to master wallet
    /// </summary>
    [Fact]
    public async Task Create_Mock_Wire_Payment_Should_Be_Initialize_Successful()
    {
        Logger.Information("Creating a mock payment test");
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
        wirePayment.Result.Data.TrackingRef.Should().NotBeEmpty();
        Logger.Information($"The mock wire payment was initiated successfully with TrackingRef {wirePayment.Result.Data.TrackingRef}");
        Assert.Equal(201, wirePayment.StatusCode);
    }
    
    
    
}