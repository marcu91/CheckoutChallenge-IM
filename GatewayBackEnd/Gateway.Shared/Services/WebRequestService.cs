using Gateway.Shared.Interfaces;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Shared.Services
{
    public class WebRequestService : IWebRequestService
    {
        public async Task<HttpResponseMessage> MakeAsyncRequest(string url, string content)
        {
            if (url == null || content == null)
                return await Task.FromResult(default(HttpResponseMessage)).ConfigureAwait(false);

            var headerType = "Content-Type: application/json";
            var mediaType = "application/json";
            HttpClient client = this.GetSetupHttpClient(url, headerType, mediaType);
            var encodedContent = new StringContent(content, Encoding.UTF8, mediaType);
            return await client.PostAsync(client.BaseAddress, encodedContent).ConfigureAwait(false);
        }

        private HttpClient GetSetupHttpClient(string url, string headerType, string mediaType)
        {
            var client = new HttpClient();
            client.Timeout = new TimeSpan(0, 5, 0);
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.TryAddWithoutValidation(headerType, mediaType);
            return client;
        }
    }
}