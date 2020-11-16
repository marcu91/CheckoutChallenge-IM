using System.Net.Http;
using System.Threading.Tasks;

namespace Gateway.Shared.Interfaces
{
    public interface IWebRequestService
    {
        Task<HttpResponseMessage> MakeAsyncRequest(string url, string content);
    }
}
