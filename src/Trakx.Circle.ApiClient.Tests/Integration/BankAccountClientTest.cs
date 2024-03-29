
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace Trakx.Circle.ApiClient.Tests.Integration;

/// <summary>
/// Bank account client test
/// <see cref="BankAccountClientTest"/>
/// </summary>
public class BankAccountClientTest : CircleClientTestsBase
{
    private readonly IBankAccountsClient _bankAccountsClient;
    private readonly IPaymentsClient _paymentsClient;
    private readonly MockCreator _mockCreator;
    
    /// <summary>
    /// constructor for <see cref="BankAccountClientTest"/>
    /// </summary>
    /// <param name="apiFixture"></param>
    /// <param name="output"></param>
    public BankAccountClientTest(CircleApiFixture apiFixture, ITestOutputHelper output) : base(apiFixture, output)
    {
        _bankAccountsClient = ServiceProvider.GetRequiredService<IBankAccountsClient>();
        _paymentsClient = ServiceProvider.GetRequiredService<IPaymentsClient>();
        _mockCreator = new MockCreator(output);
    }

    /// <summary>
    /// creating IBAN bank account with valid payload should be successful
    /// <remarks>
    /// The documentation state that the response code should be 201, but the sandbox response returned 200
    /// either 200 or 201 should be valid
    /// </remarks>
    /// </summary>
    [Fact(Skip = "IBAN not working currently for IBAN")]
    public async Task Create_IBAN_Bank_Account_Should_be_Successful()
    {
        Logger.Information("Running test for creating new IBAN Bank");
        var bankRequest = _mockCreator.WireCreationRequestIban();
        var bankCreated =  await _bankAccountsClient.CreateWireBankAccountAsync(bankRequest);

        bankCreated.Should().NotBeNull();
        bankCreated?.Content.Data.Id.Should().NotBeEmpty();

        Logger.Information("IBAN bank was created successfully with {DataId}", bankCreated?.Content.Data.Id);

        var statusCode = new HttpResponseMessage((System.Net.HttpStatusCode)bankCreated!.StatusCode);

        statusCode.IsSuccessStatusCode.Should().BeTrue();
    }

    /// <summary>
    /// Retrieving IBAN bank account with valid id
    /// </summary>
    [Fact(Skip = "Skip not working currently for IBAN")]
    public async Task Get_IBAN_Bank_Account_Should_be_Successful()
    {

        var bankRequest = _mockCreator.WireCreationRequestIban();
        var bank =  await _bankAccountsClient.CreateWireBankAccountAsync(bankRequest);
        var id = bank.Content.Data.Id;
        Logger.Information("Retrieving back account for wire transfer with id {Id}", id);
        var result =  await _bankAccountsClient.GetWireBankAccountAsync(id.GetValueOrDefault());

        result.Should().NotBeNull();
        result?.Content.Data.Id.Should().Be(id);
        Logger.Information("Bank account returned with status code {ResultStatusCode}", result?.StatusCode);

        result?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    /// <summary>
    /// Test to initiate a mock wire payment that mimics the behavior of funds sent through the bank (wire) account linked to master wallet
    /// </summary>
    [Fact(Skip = "Iban payment not working currently for IBAN")]
    public async Task Create_Mock_Wire_Payment_Should_Be_Initialize_Successful()
    {

        Logger.Information("Creating a mock payment test");
        var bankRequest = _mockCreator.WireCreationRequestIban();
        var bank = await _bankAccountsClient.CreateWireBankAccountAsync(bankRequest);
        var id = bank.Content.Data.Id;
        var result =  await _bankAccountsClient.GetWireInstructionsBankAccountAsync(id.GetValueOrDefault());
        var wireRequest = new MockWirePaymentRequest
        {
            Amount = _mockCreator.GetMoney(),
            TrackingRef = result.Content.Data.TrackingRef,
            AccountNumber = result.Content.Data.BeneficiaryBank.AccountNumber,
        };
        Logger.Information("Creating a mock payment with {TrackingRef}. AccountNumber: {AccountNumber}",
            wireRequest.TrackingRef, wireRequest.AccountNumber);
        // act
        var wirePayment = await _paymentsClient.CreateWirePaymentAsync(wireRequest);

        //assert
        wirePayment.Should().NotBeNull();
        wirePayment.Content.Data.TrackingRef.Should().NotBeEmpty();
        Logger.Information("The mock wire payment was initiated successfully with TrackingRef {DataTrackingRef}", wirePayment.Content.Data.TrackingRef);
        wirePayment.StatusCode.Should().Be(StatusCodes.Status201Created);
    }
    
    
    

}
