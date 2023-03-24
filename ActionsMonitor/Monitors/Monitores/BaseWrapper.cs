using Monitor = ActionsMonitor.Model.Monitor;

namespace ActionsMonitor.Monitors.Monitores;

public abstract class BaseWrapper
{
    protected Monitor Monitor { get; set; }
    private static readonly Dictionary<string, Type> MonitorsMap = new();
    
    static BaseWrapper()
    {
        MonitorsMap.Add("Brapi", typeof(Brapi));
        MonitorsMap.Add("Bolsa", typeof(Bolsa));
    }
    
    protected BaseWrapper(Monitor monitor)
    {
        Monitor = monitor;
    }

    public abstract Task Run();

    public static BaseWrapper Get(Monitor monitor)
    {
        if (!MonitorsMap.ContainsKey(monitor.Name!))
            throw new Exception("Monitor invalido");

        var ci = MonitorsMap[monitor.Name!].GetConstructor(new []{ typeof(Monitor) });
        return (BaseWrapper)ci?.Invoke(new object[]{ monitor })!;
    }
}