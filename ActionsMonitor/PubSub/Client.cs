namespace ActionsMonitor.PubSub;

public class Client
{
    public string Name { get; set; }
    public Func<string, string, string, Task> MessageReceivedAsync { get; set; }
}