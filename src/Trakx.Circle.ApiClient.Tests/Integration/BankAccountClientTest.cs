
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Trakx.Utils.Apis;

namespace Trakx.Circle.ApiClient.Tests.Integration;

/// <summary>
/// Bank account client test
/// <see cref="BankAccountClientTest"/>
/// </summary>
public class BankAccountClientTest : CircleClientTestsBase
{
    private readonly IBankAccountsClient _bankAccountsClient;
    private readonly IPaymentsClient _paymentsClient;
    private readonly CircleMockCreator _circleMockCreator;
    /// <summary>
    /// constructor for <see cref="BankAccountClientTest"/>
    /// </summary>
    /// <param name="apiFixture"></param>
    /// <param name="output"></param>
    public BankAccountClientTest(CircleApiFixture apiFixture, ITestOutputHelper output) : base(apiFixture, output)
    {
        _bankAccountsClient = ServiceProvider.GetRequiredService<IBankAccountsClient>();
        _paymentsClient = ServiceProvider.GetRequiredService<IPaymentsClient>();
        _circleMockCreator = new CircleMockCreator(output);
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
        var bankRequest = _circleMockCreator.GetWireCreationRequestUs;
        Response<Response15>? bankCreated =  await _bankAccountsClient.CreateWireBankAccountAsync(bankRequest);

        bankCreated.Should().NotBeNull();
        bankCreated?.Result.Data.Id.Should().NotBeEmpty();
        
        Logger.Information("Us bank was created successfully with {DataId}", bankCreated?.Result.Data.Id);

        var statusCode = new HttpResponseMessage((System.Net.HttpStatusCode)bankCreated!.StatusCode);

        statusCode.IsSuccessStatusCode.Should().BeTrue();
    }
    
    /// <summary>
    /// Retrieving US bank account with valid id
    /// </summary>
    [Fact]
    public async Task Get_Us_Bank_Account_Should_be_Successful()
    {
         
        var bankRequest = _circleMockCreator.GetWireCreationRequestUs;
        var bank =  await _bankAccountsClient.CreateWireBankAccountAsync(bankRequest);
        var id = bank.Result.Data.Id;
        Logger.Information("Retrieving back account for wire transfer with id {Id}", id);
        var result =  await _bankAccountsClient.GetWireBankAccountAsync(id.GetValueOrDefault());

        result.Should().NotBeNull();
        result?.Result.Data.Id.Should().Be(id);
        Logger.Information("Bank account returned with status code {ResultStatusCode}", result?.StatusCode);
        
        result?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    /// <summary>
    ///     Test to initiate a mock wire payment that mimics the behavior of funds sent through the bank (wire) account linked to master wallet
    /// </summary>
    [Fact]
    public async Task Create_Mock_Wire_Payment_Should_Be_Initialize_Successful()
    {
       
        Logger.Information("Creating a mock payment test");
        var bankRequest = _circleMockCreator.GetWireCreationRequestUs;
        var bank =   _bankAccountsClient.CreateWireBankAccountAsync(bankRequest).Result;
        var id = bank.Result.Data.Id;
        var result =  await _bankAccountsClient.GetWireInstructionsBankAccountAsync(id.GetValueOrDefault());
        var wireRequest = new MockWirePaymentRequest
        {
            Amount = new Money
            {
                Amount = $"{_circleMockCreator.GetDecimals()}.00",
                Currency = "USD"
            },
            TrackingRef = result.Result.Data.TrackingRef,
            AccountNumber = result.Result.Data.BeneficiaryBank.AccountNumber
        };
        Logger.Information("Creating a mock payment with {TrackingRef}. AccountNumber: {AccountNumber}", wireRequest.TrackingRef, wireRequest.AccountNumber);
        // act
        var wirePayment = await _paymentsClient.CreateWirePaymentAsync(wireRequest);
        
        //assert
        wirePayment.Should().NotBeNull();
        wirePayment.Result.Data.TrackingRef.Should().NotBeEmpty();
        Logger.Information("The mock wire payment was initiated successfully with TrackingRef {DataTrackingRef}", wirePayment.Result.Data.TrackingRef);
        wirePayment.StatusCode.Should().Be(StatusCodes.Status201Created);
    }

}
