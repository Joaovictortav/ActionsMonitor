using ActionsMoniotr.Model.DTO.Brapi;
using ActionsMonitor.PubSub;
using Newtonsoft.Json;

namespace ActionsMonitor.Monitors.Algorithms;

public class Fibonacci : Algorithm
{
    public static Task Run()
    {
        var c = PubSubServer.Registry(new Client { Name = "Fibonacci" }, new List<string> { "Calculate.Fibonacci" });
        c.MessageReceivedAsync = MessageReceivedAsync;

        return Task.CompletedTask;
    }

    private static Task MessageReceivedAsync(string client, string topic, string message)
    {
        var obj = JsonConvert.DeserializeObject<Content>(message);
        PubSubServer.SendMessageInQueue($"{obj!.monitor}.Result.Fibonacci", $"{RandomAction()} {obj.symbol}");
        return Task.CompletedTask;
    }
}