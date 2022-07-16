using System.Runtime.InteropServices;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Circle.ApiClient.Tests.Integration;

public class PaymentsClientTests : CircleClientTestsBase
{
    private readonly IPaymentsClient _paymentsClient;
    private readonly MockCreators _mockCreator;
    public PaymentsClientTests(CircleApiFixture apiFixture, ITestOutputHelper output) : base(apiFixture, output)
    {
        _paymentsClient = ServiceProvider.GetRequiredService<IPaymentsClient>();
        _mockCreator = new MockCreators(output);
    }

    
    /// <summary>
    /// retrieving payment list with valid url should return status code 200
    /// </summary>
    [Fact]
    public async Task GetPayment_Should_work()
    {
        Logger.Information("Retrieving list of payment test");
        var payment = await _paymentsClient.GetPaymentsAsync(pageSize: 10);
        payment.Should().NotBeNull();
        Logger.Information($"The call to retrieve  returned with status code {payment.StatusCode}");
        payment.StatusCode.Should().Be(200);
        
    }

    /// <summary>
    /// creating payment with valid payment payload should be successful
    /// </summary>
    [Fact]
    public async Task Create_Payment_should_be_successful()
    {
        // arrange 
        var cardPaymentCreationRequest = _mockCreator.CreatePaymentRequest();
       
        // act 
        var payment = await _paymentsClient.CreatePaymentAsync(cardPaymentCreationRequest);
        
        // assert 
       
        Assert.Equal(201,payment.StatusCode);
    }

    /// <summary>
    /// when getting a payment with valid id should return valid payment
    /// </summary>
    [Fact]
    public async Task Get_Payment_by_Valid_Id_Should_Return_Payment()
    {
        Logger.Information("starting test to retrieve payment with valid id");
        var paymentRequest = _mockCreator.CreatePaymentRequest();
        var payment = await _paymentsClient.CreatePaymentAsync(paymentRequest);
        var id = payment?.Result.Data.Id.ToString();
        Logger.Information($"Preparing to retrieve payment with id {id}");
        var paymentById = await _paymentsClient.GetPaymentAsync(id);

        paymentById.Should().NotBeNull();
        Logger.Information("Payment retrieved returned with", paymentById);
        Assert.Equal(200,paymentById.StatusCode);
        Assert.Equal(id,payment?.Result.Data.Id.ToString());
        
    }
    /// <summary>
    /// when payment is npt found with invalid id should throw not found exception
    /// </summary>
    [Fact]
    public async Task Get_Payment_by_InValid_Id_Should_Throw_404()
    {
        var id = _mockCreator.GetUid();
        var error = await  Assert.ThrowsAsync<ApiException<Error>>(async () => await _paymentsClient.GetPaymentAsync(id));
        Assert.Equal(404,error?.StatusCode);
        
    }
    

}