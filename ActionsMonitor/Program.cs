using ActionsMonitor.Controller;
using ActionsMonitor.Monitors.Algorithms;
using ActionsMonitor.PubSub;

await Task.WhenAll(new List<Task>
{
    Task.Run(PubSubServer.Start),
    Task.Run(MonitorController.Start),
    Task.Run(Startup.Start),
});