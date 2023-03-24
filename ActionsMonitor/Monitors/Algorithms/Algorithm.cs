namespace ActionsMonitor.Monitors.Algorithms;

public class Algorithm
{
    protected static string RandomAction()
    {
        int randomNumber = new Random().Next(1, 4);

        return randomNumber switch
        {
            1 => "Comprar ação:",
            2 => "Vender ação:",
            _ => "Manter ação:"
        };
    }
}