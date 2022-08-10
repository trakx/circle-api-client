using System.Text;
using Trakx.Utils.Testing;
using Xunit.Abstractions;

namespace Trakx.Circle.ApiClient.Tests.Integration;

public class MockCreators: MockCreator
{
    const int StringSize = 10;
    const int RandomGenerateMin = 10_000_000;
    const int RandomGenerateMax = 99_999_999;
    public MockCreators(ITestOutputHelper output) : base(output)
    {
    }
    /// <summary>
    /// Generate new UID
    /// </summary>
    /// <returns><see cref="string"/></returns>
    public string GetUid => $"UID{GetString(StringSize).ToUpperInvariant()}";

    /// <summary>
    /// Generate an IP
    /// </summary>
    /// <returns><see cref="string"/></returns>
    private string GetIp() => new System.Net.IPAddress(1_694_542_016).ToString();
    

    /// <summary>
    /// Card sample
    /// </summary>
    /// <returns></returns>
    private const string SampleCard = "4263982640269299";

    private string CardBase64String()
    {
        var plainText = Encoding.UTF8.GetBytes(SampleCard + Cvv);

        return Convert.ToBase64String(plainText);
    }

    private const string Cvv = "837";
    private const string SanFrancisco = "SAN FRANCISCO";
    private const string MoneyStreet = "100 Money Street";
    private const int ExpMonth = 2;
    private const int ExpYear = 2023;

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
        AccountNumber = $"{Random.Next(RandomGenerateMin,RandomGenerateMax)}",
        BankAddress = BankAddress,
        BillingDetails = BillingDetails,
        IdempotencyKey = Guid.NewGuid(),
        RoutingNumber = "121000248",

    };
    
    /// <summary>
    /// Create Not US Bank with IBAN support
    /// </summary>
    /// <returns></returns>
    public WireCreationRequest_US GetWireCreationRequestNonUs() => new ()
    {
        AccountNumber = $"{Random.Next(RandomGenerateMin,RandomGenerateMax)}",
        BankAddress = BankAddress,
        BillingDetails = BillingDetails,
        IdempotencyKey = Guid.NewGuid(),
        RoutingNumber = "121000248"

    };

    private static BillingDetails BillingDetails =>
        new()
        {
            Name = "Satoshi Nakamoto",
            City = "Boston",
            Country = "US",
            Line1 = MoneyStreet,
            District = "MA",
            PostalCode = "01234"
        };

    private static BankAddress BankAddress =>
        new()
        {
            BankName = SanFrancisco,
            City = SanFrancisco,
            Country = "US",
            District = "CA",
            Line1 = MoneyStreet,
            Line2 = "Suite 1"
        };

    /// <summary>
    /// Create a card request payload
    /// </summary>
    /// <returns><see cref="CardCreationRequest"/></returns>
    public CardCreationRequest CreatCardRequest() => new()
    {
        BillingDetails =BillingDetails,
        ExpMonth = ExpMonth,
        ExpYear = ExpYear,
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

    public SignetBankCreationRequest SignetWireCreationRequest =>
        new()
        {
            IdempotencyKey = Guid.NewGuid().ToString(),
            WalletAddress = GetEthereumAddress()
        };
}