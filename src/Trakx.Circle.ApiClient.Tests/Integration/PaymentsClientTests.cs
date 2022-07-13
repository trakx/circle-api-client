using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using Trakx.Utils.Testing;
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
        var payment = await _paymentsClient.GetPaymentsAsync();
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
    
}