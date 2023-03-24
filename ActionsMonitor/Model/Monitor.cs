using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ActionsMonitor.Model
{
	[Table("monitor")]
	public sealed class Monitor
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("ID", TypeName = "INT")] public int Id { get; set; } 
		[Column("Name", TypeName = "VARCHAR(255)"), MaxLength(255), Required] public string? Name { get; set; }
		[Column("Url", TypeName = "VARCHAR(255)"), MaxLength(255), Required] public string? Url { get; set; } 
		[Column("Active", TypeName = "BIT"), Required] public bool Active { get; set; }
		public Monitor() { }
		
		internal void Delete()
		{
			ActionsMoniotrContext.Get().MonitorsSet.Remove(this);
		}

		public static Task<List<Monitor>> List(bool active)
		{
			return ActionsMoniotrContext.Get().MonitorsSet.Where(s => s.Active == active).ToListAsync();
		}
	}
}