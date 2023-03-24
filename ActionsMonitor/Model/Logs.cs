using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActionsMonitor.Model;

[Table("logs")]
public sealed class Logs
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("Id", TypeName = "INT")] public int Id { get; set; } 
    [Column("Destiny", TypeName = "VARCHAR(255)"), Required] public string Destiny { get; set; } 
    [Column("Topic", TypeName = "VARCHAR(255)"), Required] public string Topic { get; set; } 
    [Column("Message", TypeName = "TEXT"), Required] public string Message { get; set; } 
    [Column("InclusionDate", TypeName = "DATETIME"), Required] public DateTime InclusionDate { get; set; } 

    public Logs() { }

    public Logs(string destiny, string topic, string message)
    {
        InclusionDate = DateTime.Now;
        Destiny = destiny;
        Topic = topic;
        Message = message;
			
        ActionsMoniotrContext.Get().LogsSet.Add(this);
    }
		
    internal void Delete()
    {
        ActionsMoniotrContext.Get().LogsSet.Remove(this);
    }
}