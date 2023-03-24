using ActionsMoniotr.Model.DTO.Brapi;
using ActionsMonitor.PubSub;
using ActionsMonitor.Util;
using Newtonsoft.Json;
using Action = ActionsMonitor.Model.Action;
using Monitor = ActionsMonitor.Model.Monitor;

namespace ActionsMonitor.Monitors.Monitores;

public class Brapi : BaseWrapper
{
	public Brapi(Monitor monitor ) : base(monitor) { }
	public override async Task Run()
	{
		var c = PubSubServer.Registry(new Client { Name = "Brapi" }, new List<string> { "Brapi.Result.Fibonacci", "Brapi.Result.ValorMedio" });
		c.MessageReceivedAsync = MessageReceivedAsync;
		
		var actions = await Action.List(Monitor.Id, true);

		while (true)
		{
			var param = new Dictionary<string, object>();

			param.TryAdd("range", 1);
			param.TryAdd("interval", 1);
			param.TryAdd("fundamental", "true");
			param.TryAdd("dividends", "true");
			param.TryAdd("range", 1);

			var json = await new RestClient(Monitor.Url!, "GET").Run($"quote/{string.Join("%", actions.Select(s => s.Name))}", param);
			var obj = JsonConvert.DeserializeObject<QuoteDto>(json);

			obj!.results.ForEach(s =>
			{
				s!.monitor = "Brapi";
				PubSubServer.SendMessageInQueue("Calculate.Fibonacci", JsonConvert.SerializeObject(s, Formatting.Indented));
				PubSubServer.SendMessageInQueue("Calculate.ValorMedio", JsonConvert.SerializeObject(s, Formatting.Indented));
			});
			
			Thread.Sleep(20000);
		}
	}

	private static Task MessageReceivedAsync(string client, string topic, string message)
	{
		Console.WriteLine($"Monitor - client '{client}' topic '{topic}' received message {message}");
		return Task.CompletedTask;
	}
}