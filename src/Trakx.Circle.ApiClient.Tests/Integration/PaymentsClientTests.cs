using System.Runtime.InteropServices;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Circle.ApiClient.Tests.Integration;

public class PaymentsClientTests : CircleClientTestsBase
{
    private readonly IPaymentsClient _paymentsClient;
    private readonly MockCreator _mockCreator;
    public PaymentsClientTests(CircleApiFixture apiFixture, ITestOutputHelper output) : base(apiFixture, output)
    {
        _paymentsClient = ServiceProvider.GetRequiredService<IPaymentsClient>();
        _mockCreator = new MockCreator(output);
    }

    
    [Fact]
    public async Task GetPayment_Should_work()
    {
        
        var payment = await _paymentsClient.GetPaymentsAsync(pageSize: 100);
        payment.Should().NotBeNull();
        
    }

    
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

    [Fact]
    public async Task Get_Payment_by_Valid_Id_Should_Return_Payment()
    {
        var paymentRequest = _mockCreator.CreatePaymentRequest();
        var payment = await _paymentsClient.CreatePaymentAsync(paymentRequest);
        var id = payment?.Result.Data.Id.ToString();
        var paymentById = await _paymentsClient.GetPaymentAsync(id);

        paymentById.Should().NotBeNull();
        Assert.Equal(200,paymentById.StatusCode);
        Assert.Equal(id,payment?.Result.Data.Id.ToString());
        
    }
    
    [Fact]
    public async Task Get_Payment_by_InValid_Id_Should_Throw_404()
    {
        var id = _mockCreator.GetUid();
        var error = await  Assert.ThrowsAsync<ApiException<Error>>(async () => await _paymentsClient.GetPaymentAsync(id));
        Assert.Equal(404,error?.StatusCode);
        
    }
    

}