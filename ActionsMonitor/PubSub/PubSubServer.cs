using System.Collections.Concurrent;
using System.Globalization;
using ActionsMoniotr.Model;
using ActionsMonitor.Model;

namespace ActionsMonitor.PubSub;

public abstract class PubSubServer
{
    private static readonly ConcurrentDictionary<Client, HashSet<string>> Clients = new();
    private static readonly ConcurrentDictionary<DateTime, Dictionary<string, string>> Channels = new();

    public static Client Registry(Client client, List<string> topics)
    {
        if (!Clients.ContainsKey(client))
            Clients.TryAdd(client,new HashSet<string>());
        
        Clients[client].UnionWith(topics);

        return Clients.FirstOrDefault(s => s.Key.Name == client.Name).Key;
    }
    
    public static void SendMessageInQueue(string topic, string mensage)
    {
        var value = new Dictionary<string, string>();
        value.Add(topic, mensage);
        
        Channels.TryAdd(DateTime.Now, value);
    }

    private static Task SendMessage()
    {
        while (true)
        {
            Thread.Sleep(500);
            
            var channel = Channels.OrderBy(s => s.Key).FirstOrDefault();
            var clients = Clients.Where(s => s.Value.Contains(channel.Value?.FirstOrDefault().Key.ToString(CultureInfo.InvariantCulture)!)).ToList();
            
            if (!clients.Any()) continue;
            
            foreach (var c in clients)
            {
                var key = channel.Value!.Keys.FirstOrDefault()!;
                var value = channel.Value?.FirstOrDefault().Value;
                
                InterceptMessage(c.Key.Name, key.ToString(CultureInfo.InvariantCulture)!, value!).GetAwaiter().GetResult();
                c.Key.MessageReceivedAsync(c.Key.Name, key.ToString(CultureInfo.InvariantCulture), value!);
            }

            Channels.TryRemove(channel.Key, out Dictionary<string, string> _);
        }
    }

    public static async Task Start()
    {
        await SendMessage();
    }
    private static async Task InterceptMessage(string destiny, string topic, string message)
    {
        await Logger(destiny, topic,message);
    }
    private static async Task Logger(string destiny, string topic, string message)
    {
        _ = new Logs(destiny, topic, message);
        await ActionsMoniotrContext.Get().SaveChangesAsync();
    }
}