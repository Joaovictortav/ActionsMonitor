using ActionsMoniotr.Model.DTO.Brapi;
using ActionsMonitor.PubSub;
using Newtonsoft.Json;

namespace ActionsMonitor.Monitors.Algorithms;

public class ValorMedio : Algorithm
{
    public static Task Run()
    {
        var c = PubSubServer.Registry(new Client { Name = "ValorMedio" }, new List<string> { "Calculate.ValorMedio" });
        c.MessageReceivedAsync = MessageReceivedAsync;

        return Task.CompletedTask;
    }

    private static Task MessageReceivedAsync(string client, string topic, string message)
    {
        var obj = JsonConvert.DeserializeObject<Content>(message);
        PubSubServer.SendMessageInQueue($"{obj!.monitor}.Result.ValorMedio", $"{RandomAction()} {obj.symbol}");
        return Task.CompletedTask;
    }
}