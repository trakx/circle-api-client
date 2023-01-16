using System.Text;
using Xunit.Abstractions;

namespace Trakx.Circle.ApiClient.Tests;

public class MockCreator: Trakx.Utils.Testing.MockCreator
{
    private const int StringSize = 10;
    private const int RandomGenerateMin = 10_000_000;
    private const int RandomGenerateMax = 99_999_999;
    public MockCreator(ITestOutputHelper output) : base(output)
    {
    }
    /// <summary>
    /// Generate new UID
    /// </summary>
    public string GetUid() => $"UID{GetString(StringSize).ToUpperInvariant()}";

    /// <summary>
    /// Generate an IP
    /// </summary>
    private string GetIp() => new System.Net.IPAddress(1_694_542_016).ToString();


    /// <summary>
    /// Card sample
    /// </summary>
    private const string SampleCard = "4263982640269299";

    private string GetCardBase64String()
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
    /// A sample source id from circle API sandbox
    /// </summary>
    private const string SandboxSourceId = "b8627ae8-732b-4d25-b947-1df8f4007a29";

    private const string Iban = "DE31100400480532013000";

    /// <summary>
    /// Create payment request payload
    /// </summary>
    public CardPaymentCreationRequest GetCardPaymentCreationRequest() => new()
    {
        Amount = GetMoney(),
        IdempotencyKey = Guid.NewGuid(),
        Metadata = new MetadataPayment
        {
            Email = GetEmailAddress("trakx.io"),
            PhoneNumber = "+14155555555",
            SessionId = "DE6FA86F60BB47B379307F851E238617",
            IpAddress = $"{GetIp()}"
        },
        Verification = CardPaymentCreationRequestVerification.None,

        Source = new Source { Id = SandboxSourceId },
        Description = "Payment",
    };

    public Money GetMoney() => new()
    {
            Amount = $"{GetDecimals()}.00",
            Currency = "USD"
        };

    /// <summary>
    /// Create Us Wire Bank account payload
    /// </summary>
    public WireCreationRequest_iban WireCreationRequestIban() => new()
    {
        Iban = Iban,
        BankAddress = BankAddress,
        BillingDetails = BillingDetails,
        IdempotencyKey = Guid.NewGuid(),
       
    };

    private static BillingDetails BillingDetails =>
        new()
        {
            Name = "Satoshi Nakamoto",
            City = "Boston",
            Country = "FRA",
            Line1 = MoneyStreet,
            District = "MA",
            PostalCode = "01234"
        };

    private static BankAddressIbanSupported BankAddress =>
        new()
        {
            BankName = SanFrancisco,
            City = SanFrancisco,
            Country = "FRA",
            District = "CA",
            Line1 = MoneyStreet,
            Line2 = "Suite 1"
        };

    /// <summary>
    /// Create a card request payload
    /// </summary>
    /// <returns><see cref="CardCreationRequest"/></returns>
    public CardCreationRequest GetCardCreationRequest() => new()
    {
        BillingDetails = BillingDetails,
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
        EncryptedData = GetCardBase64String(),
    };

    public SignetBankCreationRequest GetSignetWireCreationRequest() =>
        new()
        {
            IdempotencyKey = Guid.NewGuid().ToString(),
            WalletAddress = GetEthereumAddress()
        };

    public SilverGateSenBankRequest GetSilverGateSenBankRequest =>
        new()
        {
            IdempotencyKey = Guid.NewGuid().ToString(),
           Currency = Currency.USD.ToString(),
           AccountNumber =  $"{Random.Next(RandomGenerateMin,RandomGenerateMax)}"
        };
    public  SilverGateSenBankTransferRequest GetSilverGateSenBankTransferRequest(string trackingRef) =>
        new()
        {
            Amount = GetMoney(),
            BeneficiaryBank = new BeneficiaryBank
            {
                AccountNumber = "11111111",

            },
            TrackingRef = trackingRef
        };
}
