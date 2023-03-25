await Task.WhenAll(new List<Task>
{
    Task.Run(ActionsMonitor.PubSub.PubSubServer.Listener),
    Task.Run(ActionsMonitor.Controller.MonitorController.Start),
    Task.Run(ActionsMonitor.Monitors.Algorithms.Startup.Start),
});