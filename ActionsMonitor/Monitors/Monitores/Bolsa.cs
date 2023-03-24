using ActionsMonitor.PubSub;
using Action = ActionsMonitor.Model.Action;
using Monitor = ActionsMonitor.Model.Monitor;

namespace ActionsMonitor.Monitors.Monitores;

public class Bolsa : BaseWrapper
{
	public Bolsa(Monitor monitor ) : base(monitor) { }
	public override async Task Run()
	{
		var c = PubSubServer.Registry(new Client { Name = "Bolsa" }, new List<string> { "Bolsa.Result.Fibonacci", "Bolsa.Result.ValorMedio" });
		c.MessageReceivedAsync = MessageReceivedAsync;
		
		var actions = await Action.List(Monitor.Id, true);
	}
	
	private static Task MessageReceivedAsync(string client, string topic, string message)
	{
		Console.WriteLine($"Monitor - client '{client}' topic '{topic}' received message {message}");
		return Task.CompletedTask;
	}
}