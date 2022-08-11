using Microsoft.Extensions.Options;
using Serilog;
using Trakx.Utils.Apis;

namespace Trakx.Circle.ApiClient.Utils;

public interface ICircleCredentialsProvider : ICredentialsProvider { }
public sealed class ApiKeyCredentialsProvider : ICircleCredentialsProvider, IDisposable
{
    private readonly CircleApiConfiguration _configuration;
    private readonly CancellationTokenSource _tokenSource;

    private static readonly ILogger Logger = Log.Logger.ForContext<ApiKeyCredentialsProvider>();

    public ApiKeyCredentialsProvider(CircleApiConfiguration configuration)
    {
        _configuration = configuration;
        _tokenSource = new CancellationTokenSource();
    }


    #region Implementation of ICredentialsProvider

    /// <inheritdoc />
    public void AddCredentials(HttpRequestMessage msg)
    {
        msg.Headers.Add("Authorization", $"Bearer {_configuration.ApiKey}");
        Logger.Verbose("Headers added");
    }

    public Task AddCredentialsAsync(HttpRequestMessage msg)
    {
        return Task.Run(() => AddCredentials(msg));
    }

    #endregion

    #region IDisposable

    private void Dispose(bool disposing)
    {
        if (!disposing) return;
        _tokenSource.Cancel();
        _tokenSource.Dispose();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}
