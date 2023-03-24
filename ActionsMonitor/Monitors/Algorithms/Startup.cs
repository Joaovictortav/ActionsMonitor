namespace ActionsMonitor.Monitors.Algorithms;

public class Startup
{
    public static async Task Start()
    {
        await Task.WhenAll(new List<Task>()
        {
            Task.Run(ValorMedio.Run),
            Task.Run(Fibonacci.Run),
        });
    }
}