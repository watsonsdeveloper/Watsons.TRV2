using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Watsons.TRV2.Services.RTS
{
    public class RtsService : IRtsService
    {
        private readonly RtsSettings _rtsSettings;
        private readonly HttpClient _httpClient;
        
        public RtsService(IOptions<RtsSettings> rtsSettings)
        {
            _rtsSettings = rtsSettings.Value;
            _httpClient = new HttpClient();
        }
        public async Task<Dictionary<string, int>?> GetMultipleProductSingleStore(DTO.GetMultipleProductSingleStore.Request request)
        {
            try
            {
                Dictionary<string, int>? result = null;

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
                var productStockList = JsonSerializer.Deserialize<List<DTO.GetMultipleProductSingleStore.Response>>(responseContent);
                if (productStockList != null)
                {
                    result = productStockList.Where(p => p.Plu != null).ToDictionary(p => p.Plu!, p => (int)p.AvailableStock);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting multiple product single store", ex);
            }
        }
        
    }
}
