using System.Web;

namespace ActionsMonitor.Util;

public class RestClient
{
    private static string _baseUrl = string.Empty;
    private static readonly HashSet<string> Methods = new(){"GET", "POST"};
    private readonly HttpClient _client = new (new HttpClientHandler()) {Timeout = TimeSpan.FromSeconds(900)};
    private readonly HttpRequestMessage _requestMessage = new ();
    public RestClient(string baseUrl, string method)
    {
        if (!Methods.Contains(method))
            throw new Exception("Method invalid");

        _baseUrl = baseUrl;
        _requestMessage.Method = new HttpMethod(method);
    }
    internal async Task<string> Run(string path = null, Dictionary<string, object> qs = null)
    {
        
        var url = _baseUrl + path + "?";
        
        if (qs is {Count: > 0})
        {
            var paramsCount = 1;
            foreach (var value in qs)
            {
                if (paramsCount > 1)
                    url += "&";
                switch (value.Value)
                {
                    case null:
                        continue;
                    case DateTime time:
                        url += $"{value.Key}={time:yyyy-MM-dd HH:mm:ss}&";
                        break;
                    default:
                        url += $"{value.Key}={HttpUtility.UrlEncode(value.Value.ToString())}";
                        break;
                }
                paramsCount++;
            }
        }
        
        var result = string.Empty;
        var client = new HttpClient();

        _requestMessage.RequestUri = new Uri(url);

        try
        {
            var response = await _client.SendAsync(_requestMessage);

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            else
                throw new Exception("Error");
            
        }
        catch (Exception e)
        {
            return result;
        }
        finally
        {
            client.Dispose();
        }
        
        return result;
    }
}