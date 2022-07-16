using System.Net;
using System.Text;
using Trakx.Utils.Testing;
using Xunit.Abstractions;

namespace Trakx.Circle.ApiClient.Tests.Integration;

public class MockCreators: MockCreator
{
    public MockCreators(ITestOutputHelper output) : base(output)
    {
    }
    /// <summary>
    /// Generate new UIS
    /// </summary>
    /// <returns><see cref="string"/></returns>
    public string GetUid() => $"UID{GetString(10).ToUpperInvariant()}";

    /// <summary>
    /// Generate an IP
    /// </summary>
    /// <returns><see cref="string"/></returns>
    private string GetIp() => new IPAddress(1694542016).ToString();
    

    /// <summary>
    /// Card sample
    /// </summary>
    /// <returns></returns>
    private string SampleCard() => "4263982640269299";

    private string CardBase64String()
    {
        var plainText = Encoding.UTF8.GetBytes(SampleCard() + Cvv());

        return Convert.ToBase64String(plainText);
    }

    private string Cvv() => "837";
    /// <summary>
    /// Create payment request payload
    /// </summary>
    /// <returns> <see cref="CardCreationRequest"/></returns>
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

    /// <summary>
    /// Create Us Wire Bank account payload
    /// </summary>
    /// <returns><seealso cref="WireCreationRequest_US"/></returns>
    public WireCreationRequest_US WireCreationRequestUs() => new()
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
    
    /// <summary>
    /// Create Not US Bank with IBAN support
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Create a card request payload
    /// </summary>
    /// <returns><see cref="CardCreationRequest"/></returns>
    public CardCreationRequest CreatCardRequest() => new()
    {
        BillingDetails = new BillingDetails
        {
            City = "Boston",
            Country = "US",
            Name = "Satoshi Nakamoto",
            Line1 = "100 Money Street",
            PostalCode = "01234",


        },
        ExpMonth = 2,
        ExpYear = 2023,
        Metadata = new MetadataCard
        {
            Email = GetEmailAddress("track.io"),
            PhoneNumber = "+14155555555",
            SessionId = "DE6FA86F60BB47B379307F851E238617",
            IpAddress = GetIp()
        },
        IdempotencyKey = Guid.NewGuid(),
        EncryptedData = new EncryptedCardPaymentData
        {
            Cvv = $"{CardBase64String()}"
        }

    };
}