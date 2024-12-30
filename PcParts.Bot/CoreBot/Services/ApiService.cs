#nullable enable
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CoreBot.Services
{
    public static class ApiService<T>
    {
        private static readonly string BASE_URL = "https://pcpartsapi.azurewebsites.net/api/";
        static HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(60) };

        public static async Task<T?> GetAsync(string endPoint)
        {
            try
            {
                string url = BASE_URL + endPoint;
                var response = await client.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(jsonData))
                    {
                        return JsonConvert.DeserializeObject<T>(jsonData);
                    }
                    else
                    {
                        throw new Exception("Resource Not Found");
                    }
                }
                else if (response.StatusCode ==
                         System.Net.HttpStatusCode.NotFound)
                {
                    return default;
                }
                else
                {
                    throw new Exception("Request failed with status code " + response.StatusCode);
                }
            }
            catch
            {
                throw;
            }
        }

        public static async Task<TU> PostAsync<TU>(string endPoint, T data)
        {
            try
            {
                string url = BASE_URL + endPoint;
                var response = await client.PostAsJsonAsync(url, data);
                if (response.StatusCode != System.Net.HttpStatusCode.Created
                    && response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("Request failed with status code " + response.StatusCode);
                }
                return JsonConvert.DeserializeObject<TU>(await response.Content.ReadAsStringAsync());
            }
            catch
            {
                throw;
            }
        }

        public static async Task PutAsync(string endPoint, T data)
        {
            try
            {
                string url = BASE_URL + endPoint;
                var response = await client.PutAsJsonAsync(url, data);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("Request failed with status code " + response.StatusCode);
                }
            }
            catch
            {
                throw;
            }
        }

        public static async Task DeleteAsync(string endPoint)
        {
            try
            {
                string url = BASE_URL + endPoint;
                var response = await client.DeleteAsync(url);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("Request failed with status code " + response.StatusCode);
                }
            }
            catch
            {
                throw;
            }
        }

    }
}