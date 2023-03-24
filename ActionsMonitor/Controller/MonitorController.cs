using ActionsMonitor.Model;
using ActionsMonitor.Monitors.Monitores;
using Monitor = ActionsMonitor.Model.Monitor;

namespace ActionsMonitor.Controller;

public abstract class MonitorController
{
    public static async Task Start()
    {
        var monitors = await Monitor.List(true);

        var tasks = new List<Task>();
        monitors.ForEach(monitor => tasks.Add(Task.Run(() => Start(monitor))));

        await Task.WhenAll(tasks);
    }
    
    private static async Task Start(Monitor monitor)
    {
        ActionsMoniotrContext.ResetContext();
        await using var context = ActionsMoniotrContext.Get();

        await Task.Run(() => BaseWrapper.Get(monitor).Run());
    }
}