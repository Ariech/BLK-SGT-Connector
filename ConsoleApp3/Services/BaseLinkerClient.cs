using MyBaseLinkerProject.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;

public class BaseLinkerClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiToken;

    public BaseLinkerClient(string apiToken)
    {
        _apiToken = apiToken;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("X-BLToken", _apiToken);
    }

    public async Task<List<Order>> GetOrders(List<int> orderIds)
    {
        if (orderIds == null || orderIds.Count == 0)
            return new List<Order>();

        string joinedIds = string.Join(",", orderIds);
        string method = "getOrders";
        string parametersJson = JsonConvert.SerializeObject(new { order_id = joinedIds });


        var postData = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("method", method),
            new KeyValuePair<string, string>("parameters", parametersJson)
        });

        HttpResponseMessage response = await _httpClient.PostAsync("https://api.baselinker.com/connector.php", postData);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        //Console.WriteLine(json);
        var deserialized = JsonConvert.DeserializeObject<OrderResponse>(json);

        if (deserialized == null)
            throw new Exception("Nie udało się zdeserializować odpowiedzi z BaseLinkera.");

        return deserialized.orders ?? new List<Order>();
    }

    public List<Order> GetOrdersSync(List<int> orderIds)
    {
        return GetOrders(orderIds).GetAwaiter().GetResult();
    }
}
