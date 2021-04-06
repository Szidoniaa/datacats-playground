using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Equinor.OmniaDataCatalogApi.Collibra.Dtos;
using Newtonsoft.Json;

namespace Equinor.OmniaDataCatalogApi.Collibra.HttpClients
{
    public interface ICollibraHttpClient
    {
        Task<CommunitiesResultDto> Get(string path, Dictionary<string, string> headers,
            CancellationToken cancellationToken);
    }

    public class CollibraHttpClient : ICollibraHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _collibraUri = "https://equinor-dev.collibra.com/";
        public CollibraHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("collibraClient");
        }
        public async Task<CommunitiesResultDto> Get(string path, Dictionary<string, string> headers,
            CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                _collibraUri + path);
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            var httpResponseMessage = await _httpClient.SendAsync(request, cancellationToken);
            var responseJson = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CommunitiesResultDto>(responseJson);
        }
    }
}