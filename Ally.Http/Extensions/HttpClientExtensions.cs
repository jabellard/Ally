using System;
using System.Net.Http;
using Ally.Http.Options;
using Microsoft.Extensions.Configuration;

namespace Ally.Http.Extensions
{
    public static class HttpClientExtensions
    {
        public static HttpClient Configure(this HttpClient httpClient, IConfigurationSection configuration)
        {

            var httpClientOptions = new HttpClientOptions();
            configuration.Bind(httpClientOptions);
            return httpClient.Configure(httpClientOptions);
        }
        
        public static HttpClient Configure(this HttpClient httpClient, HttpClientOptions httpClientOptions)
        {

            return httpClient.Map(httpClientOptions);
        }

        private static HttpClient Map(this HttpClient httpClient, HttpClientOptions httpClientOptions)
        {
            if (httpClientOptions.BaseAddress != null)
                httpClient.BaseAddress = new Uri(httpClientOptions.BaseAddress);
            if (httpClientOptions.DefaultRequestHeaders != null)
                foreach (var (key, value) in httpClientOptions.DefaultRequestHeaders)
                    httpClient.DefaultRequestHeaders.Add(key, value);
            if (httpClientOptions.MaxResponseContentBufferSize != null)
                httpClient.MaxResponseContentBufferSize = (long) httpClientOptions.MaxResponseContentBufferSize;
            if (httpClientOptions.TimeOutInSeconds != null)
                httpClient.Timeout = new TimeSpan(0, 0, (int)httpClientOptions.TimeOutInSeconds);
            return httpClient;
        }
    }
}