﻿using System;
using System.Net.Http;
using System.Threading;
using Microsoft.Extensions.Options;
using Serilog;
using Trakx.Utils.Apis;

namespace Trakx.Circle.ApiClient.Utils
{
    public interface ICircleCredentialsProvider : ICredentialsProvider { };
    public class ApiKeyCredentialsProvider : ICircleCredentialsProvider, IDisposable
    {
        private readonly CircleApiConfiguration _configuration;
        private readonly CancellationTokenSource _tokenSource;

        private static readonly ILogger Logger = Log.Logger.ForContext<ApiKeyCredentialsProvider>();

        public ApiKeyCredentialsProvider(IOptions<CircleApiConfiguration> configuration)
        {
            _configuration = configuration.Value;
            _tokenSource = new CancellationTokenSource();
        }

        
        #region Implementation of ICredentialsProvider

        /// <inheritdoc />
        public void AddCredentials(HttpRequestMessage msg)
        {
            msg.Headers.Add("Authorization", "Bearer " + _configuration.ApiKey);
            Logger.Verbose("Headers added");
        }
        #endregion

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            _tokenSource.Cancel();
            _tokenSource?.Dispose();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}