using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace ActionsMonitor.Model;

public class ActionsMoniotrContext : DbContext
{
    private static AsyncLocal<ActionsMoniotrContext> _instance = new ();
    
    internal static ActionsMoniotrContext Get()
    {
        if (_instance.Value is null)
        {
            var options = new DbContextOptions<ActionsMoniotrContext>();
            _instance.Value = new ActionsMoniotrContext(options);
        }
        
        return _instance.Value;
    }
    
    public DbSet<Monitor> MonitorsSet { get; set; }
    public DbSet<Action> ActionsSet { get; set; }
    public DbSet<Logs> LogsSet { get; set; }
    internal static void ResetContext(){
        _instance.Value = null!;
    }

    public ActionsMoniotrContext(DbContextOptions<ActionsMoniotrContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = "server=financemonitor.ca2n3ugnqnaf.sa-east-1.rds.amazonaws.com;uid=admin;pwd=rootroot;database=finance_monitor";
            optionsBuilder.UseMySql(connectionString, ServerVersion.Parse("8.0.27-mysql"));
        }
    }
    
    public override void Dispose()
    {
        base.Dispose();
        _instance.Value = null!;
    }

}