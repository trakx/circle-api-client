using System.Net;
using Xunit.Abstractions;

namespace Trakx.Circle.ApiClient.Tests.Integration;

public class MockCreator: Trakx.Utils.Testing.MockCreator
{
    public MockCreator(ITestOutputHelper output) : base(output)
    {
    }
    public string GetUid() => $"UID{GetString(10).ToUpperInvariant()}";

    private string GetIp()
    {
        return new IPAddress(1694542016).ToString();
    }
    
    public CardPaymentCreationRequest CreatePaymentRequest () => new CardPaymentCreationRequest
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
}