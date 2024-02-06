using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Watsons.TRV2.Service.RTS
{
    public interface IRtsService
    {
        Task<DTO.GetMultipleProductSingleStore.Response?> GetMultipleProductSingleStore(DTO.GetMultipleProductSingleStore.Request request);
    }
    public class RtsService : IRtsService
    {
        private readonly RtsSettings _rtsSettings;
        private readonly HttpClient _httpClient;
        
        public RtsService(IOptions<RtsSettings> rtsSettings, HttpClient httpClient)
        {
            _rtsSettings = rtsSettings.Value;
            _httpClient = httpClient;
        }

        public async Task<DTO.GetMultipleProductSingleStore.Response?> GetMultipleProductSingleStore(DTO.GetMultipleProductSingleStore.Request request)
        {
            try
            {
                var url = _rtsSettings.Url + RtsApi.GetMultipleProductSingleStoreApiPath;
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                if(response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Error while getting multiple product single store");
                }
                var responseContent = await response.Content.ReadAsStringAsync();
                if(responseContent == null)
                {
                    throw new Exception("Error while getting multiple product single store");
                }
                var result = JsonSerializer.Deserialize<DTO.GetMultipleProductSingleStore.Response>(responseContent);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting multiple product single store", ex);
            }
        }
        
    }
}
