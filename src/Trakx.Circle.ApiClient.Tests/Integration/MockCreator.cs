using System.Net;
using Trakx.Utils.Testing;
using Xunit.Abstractions;

namespace Trakx.Circle.ApiClient.Tests.Integration;

public class MockCreators: MockCreator
{
    public MockCreators(ITestOutputHelper output) : base(output)
    {
    }
    public string GetUid() => $"UID{GetString(10).ToUpperInvariant()}";

    private string GetIp()
    {
        return new IPAddress(1694542016).ToString();
    }
    
    public CardPaymentCreationRequest CreatePaymentRequest () => new()
    {
        Amount = new Money
        {
            Amount = $"{GetDecimals()}.00", 
            Currency = "USD"
        },
        IdempotencyKey = Guid.NewGuid(),
        // KeyId = "key1",
        Metadata = new MetadataPayment
        {
            Email = GetEmailAddress("trakx.io"),
            PhoneNumber = "+14155555555",
            SessionId = "DE6FA86F60BB47B379307F851E238617",
            IpAddress = $"{GetIp()}"
        },
        Verification = CardPaymentCreationRequestVerification.None,
        Source = new Source { Id = "b8627ae8-732b-4d25-b947-1df8f4007a29" },
        Description = "Payment",
        // EncryptedData = new EncryptedCardPaymentData() { Cvv = "UHVibGljS2V5QmFzZTY0RW5jb2RlZA==" }

    };

    public WireCreationRequest_US WireCreationRequestUs() => new WireCreationRequest_US
    {
        AccountNumber = $"{Random.Next(10000000,99999999)}",
        BankAddress = new BankAddress
        {
            BankName = "SAN FRANCISCO",
            City = "SAN FRANCISCO",
            Country = "US",
            District = "CA",
            Line1 = "100 Money Street",
            Line2 = "Suite 1"
        },
        BillingDetails = new BillingDetails
        {
            Name = "Satoshi Nakamoto",
            City = "Boston",
            Country = "US",
            Line1 = "100 Money Street",
            District = "MA",
            PostalCode = "01234"
        },
        IdempotencyKey = Guid.NewGuid(),
        RoutingNumber = "121000248",

    };
    
    public WireCreationRequest_US NonUsBankWithIbanSupport() => new ()
    {
        AccountNumber = $"{Random.Next(10000000,99999999)}",
        BankAddress = new BankAddress
        {
            BankName = "SAN FRANCISCO",
            City = "SAN FRANCISCO",
            Country = "US",
            District = "CA",
            Line1 = "100 Money Street",
            Line2 = "Suite 1"
        },
        BillingDetails = new BillingDetails
        {
            Name = "Satoshi Nakamoto",
            City = "Boston",
            Country = "US",
            Line1 = "100 Money Street",
            District = "MA",
            PostalCode = "01234"
        },
        IdempotencyKey = Guid.NewGuid(),
        RoutingNumber = "121000248"

    };
}