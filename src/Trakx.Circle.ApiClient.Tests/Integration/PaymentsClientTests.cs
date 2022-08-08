using FluentAssertions;
using Microsoft.AspNetCore.Http;
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
        const int pageSize = 10;
        var payment = await _paymentsClient.GetPaymentsAsync(pageSize: pageSize);
        payment.Should().NotBeNull();
        Logger.Information("The call to retrieve  returned with status code {PaymentStatusCode}", payment.StatusCode);
        payment.StatusCode.Should().Be(StatusCodes.Status200OK);
        
    }
    


    /// <summary>
    /// when payment is npt found with invalid id should throw not found exception
    /// </summary>
    [Fact]
    public async Task Get_Payment_by_InValid_Id_Should_Throw_404()
    {
        var id = _mockCreator.GetUid;
        var error = await  Assert.ThrowsAsync<ApiException<Error>>(async () => await _paymentsClient.GetPaymentAsync(id));
        Assert.Equal(StatusCodes.Status404NotFound,error?.StatusCode);
        
    }
    

}
